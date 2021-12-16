using System;
using System.Drawing;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace SMM2VIEWER
{
    static class ModuleSMM
    {
        public const string UR = "198174193181024108188204033027038009110065067055059092053071110013197170026126044073185151163192252147231138210253130239234135026040040007108000240149029107071034164200170245039067011106201189022119240223";
        public static int[] MapWidth = new int[2], MapHeight = new int[2];

        public struct LvlHeader
        {
            public byte StartY;
            public byte GoalY;
            public short GoalX;
            public short Timer;
            public short ClearCA;
            public short DateYY;
            public byte DateMM;
            public byte DateDD;
            public byte DateH;
            public byte DateM;
            public byte AutoscrollSpd;
            public byte ClearCC;
            public int ClearCRC;
            public int GameVer;
            public int MFlag;
            public int ClearAttempts;
            public int ClearTime;
            public int CreationID;
            public long UploadID;
            public int ClearVer;
            public short GameStyle;
            public string Name;
            public string Desc;
        }
        // 关卡文件头H200
        public static LvlHeader LH;

        public struct MapObject
        {
            public int X;
            public int Y;
            public byte W;
            public byte H;
            public int Flag;
            public int CFlag;
            public int Ex;
            public short ID;
            public short CID;
            public short LID;
            public short SID;
            public byte LinkType;
        }

        public struct MapGround
        {
            public byte X;
            public byte Y;
            public byte ID;
            public byte BID;
        }

        public struct MapHeader
        {
            public byte Theme;
            public byte AutoscrollType;
            public byte BorFlag;
            public byte Ori;
            public byte LiqEHeight;
            public byte LiqMode;
            public byte LiqSpd;
            public byte LiqSHeight;
            public int BorR;
            public int BorT;
            public int BorL;
            public int BorB;
            public int Flag;
            public int ObjCount;
            public int SndCount;
            public int SnakeCount;
            public int ClearPipCount;
            public int CreeperCount;
            public int iBlkCount;
            public int TrackBlkCount;
            public int GroundCount;
            public int TrackCount;
            public int IceCount;
        }

        public struct MapTrack
        {
            public short UN;
            public byte Flag;
            public byte X;
            public byte Y;
            public byte Type;
            public short LID;
            public int K0;
            public int K1;
            public byte F0;
            public byte F1;
            public byte F2;
        }

        public struct MapClearPipe
        {
            public byte Index;
            public byte NodeCount;
            public MapClearPipeNode[] Node;
        }

        public struct MapClearPipeNode
        {
            public byte type;
            public byte index;
            public byte X;
            public byte Y;
            public byte W;
            public byte H;
            public byte Dir;
        }

        public struct MapSnakeBlock
        {
            public byte index;
            public byte NodeCount;
            public MapSnakeBlockNode[] Node;
        }

        public struct MapSnakeBlockNode
        {
            public byte index;
            public byte Dir;
        }

        public struct MapMoveBlock
        {
            public byte index;
            public short NodeCount;
            public MapMoveBlockNode[] Node;
        }

        public struct MapMoveBlockNode
        {
            public byte p0;
            public byte p1;
            public byte p2;
        }

        public struct MapCreeper
        {
            public byte index;
            public short NodeCount;
            public byte[] Node;
        }

        public static MapClearPipe[] MapCPipe;
        public static MapSnakeBlock[] MapSnk;
        public static int OSNK;
        public static int[,] TrackNode, GroundNode;
        public static Point[,] TrackYPt = new Point[16, 3];
        public static MapMoveBlock[] MapMoveBlk, MapTrackBlk;
        public static MapCreeper[] MapCrp;
        public static byte[] ObjLinkType;
        // 地图文件头H48
        public static MapHeader[] MH = new MapHeader[2];
        public static MapHeader MapHdr;
        public static MapObject[] MapObj;
        public static MapGround[] MapGrd;
        public static MapGround[] MapIce;
        public static MapTrack[] MapTrk;
        public static string[] ObjEng = new string[] { "栗宝宝", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币", "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW", "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖", "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头", "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "大蘑菇", "鞋子栗宝宝", "碎碎龟", "加农炮", "鱿鱿", "城堡桥", "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡", "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台", "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "USA蘑菇", "圈圈", "狼牙棒", "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币", "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记", "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖", "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿", "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床" };
        public static string[] ObjEng3 = new string[] { "栗宝宝", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币", "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW", "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖", "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头", "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "超级树叶", "鞋子栗宝宝", "碎碎龟", "加农炮", "鱿鱿", "城堡桥", "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡", "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台", "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "青蛙装", "圈圈", "狼牙棒", "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币", "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记", "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖", "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿", "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床" };
        public static string[] ObjEngW = new string[] { "栗邦邦", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币", "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW", "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖", "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头", "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "斗篷羽毛", "耀西", "碎碎龟", "加农炮", "鱿鱿", "城堡桥", "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡", "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台", "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "力量气球", "圈圈", "狼牙棒", "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币", "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记", "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖", "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿", "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床" };
        public static string[] ObjEngU = new string[] { "栗宝宝", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币", "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW", "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖", "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头", "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "螺旋桨蘑菇", "耀西", "碎碎龟", "加农炮", "鱿鱿", "城堡桥", "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡", "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台", "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "超级橡栗", "圈圈", "狼牙棒", "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币", "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记", "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖", "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿", "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床" };
        public static string[] ObjEngD = new string[] { "板栗", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币", "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW", "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖", "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头", "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "超级铃铛", "鞋/耀西", "碎碎龟", "加农炮", "鱿鱿", "城堡桥", "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡", "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台", "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "回旋镖花", "圈圈", "狼牙棒", "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币", "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记", "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖", "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿", "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床" };
        public static string PT;
        public static Point[,] TileLoc = new Point[151, 3];
        public static Point[,] PipeLoc = new Point[4, 8];
        public static Point[] GrdLoc = new Point[257];
        public static string[] Badges = new string[] { "", "金缎带", "银缎带", "铜缎带", "金牌", "银牌", "铜牌" };
        public static string[] BadgesType = new string[] { "工匠点数", "耐力挑战(简单)", "耐力挑战(普通)", "耐力挑战(困难)", "耐力挑战(极难)", "多人对战", "过关关卡数", "最先通过关卡数", "最短时间关卡数", "工匠点数(周)" };

        public static string GetItemName(int n, int v)
        {
            string GetItemNameRet = default;
            switch (v)
            {
                case 12621:
                    {
                        GetItemNameRet = ObjEng[n];
                        break;
                    }

                case 13133:
                    {
                        GetItemNameRet = ObjEng3[n];
                        break;
                    }

                case 22349:
                    {
                        GetItemNameRet = ObjEngW[n];
                        break;
                    }

                case 21847:
                    {
                        GetItemNameRet = ObjEngU[n];
                        break;
                    }

                case 22323:
                    {
                        GetItemNameRet = ObjEngD[n];
                        break;
                    }

                default:
                    {
                        GetItemNameRet = "?";
                        break;
                    }
            }

            return GetItemNameRet;
        }

        public static void LoadLvlData(string P, bool IO)
        {
            int Offset;
            Offset = Conversions.ToInteger(Interaction.IIf(IO, 0x201, 0x2E0E1));
            FileSystem.FileOpen(1, P, OpenMode.Binary);
            FileSystem.FileGet(1, ref LH.StartY, 0x0 + 1);
            FileSystem.FileGet(1, ref LH.GoalY, 0x1 + 1);
            FileSystem.FileGet(1, ref LH.GoalX, 0x2 + 1);
            FileSystem.FileGet(1, ref LH.Timer, 0x4 + 1);
            FileSystem.FileGet(1, ref LH.ClearCA, 0x6 + 1);
            FileSystem.FileGet(1, ref LH.DateYY, 0x8 + 1);
            FileSystem.FileGet(1, ref LH.DateMM, 0xA + 1);
            FileSystem.FileGet(1, ref LH.DateDD, 0xB + 1);
            FileSystem.FileGet(1, ref LH.DateH, 0xC + 1);
            FileSystem.FileGet(1, ref LH.DateM, 0xD + 1);
            FileSystem.FileGet(1, ref LH.AutoscrollSpd, 0xE + 1);
            FileSystem.FileGet(1, ref LH.ClearCC, 0xF + 1);
            FileSystem.FileGet(1, ref LH.ClearCRC, 0x10 + 1);
            FileSystem.FileGet(1, ref LH.GameVer, 0x14 + 1);
            FileSystem.FileGet(1, ref LH.MFlag, 0x18 + 1);
            FileSystem.FileGet(1, ref LH.ClearAttempts, 0x1C + 1);
            FileSystem.FileGet(1, ref LH.ClearTime, 0x20 + 1);
            FileSystem.FileGet(1, ref LH.CreationID, 0x24 + 1);
            FileSystem.FileGet(1, ref LH.UploadID, 0x28 + 1);
            FileSystem.FileGet(1, ref LH.ClearVer, 0x30 + 1);
            FileSystem.FileGet(1, ref LH.GameStyle, 0xF1 + 1);

            // VER TEST
            // LH.GameStyle = 12621

            long i, j;
            string S;
            var K = default(short);
            S = "";
            for (i = 1L; i <= 0x42L; i += 2L)
            {
                FileSystem.FileGet(1, ref K, 0xF4L + i);
                if (K == 0)
                    break;
                S += Conversions.ToString(K);
            }

            LH.Name = S;
            S = "";
            for (i = 1L; i <= 0xCAL; i += 2L)
            {
                FileSystem.FileGet(1, ref K, 0x136L + i);
                if (K == 0)
                    break;
                S += Conversions.ToString(K);
            }

            LH.Desc = S;
            long M;
            FileSystem.FileGet(1, ref MapHdr.Theme, Offset + 0x0);
            FileSystem.FileGet(1, ref MapHdr.AutoscrollType, Offset + 0x1);
            FileSystem.FileGet(1, ref MapHdr.BorFlag, Offset + 0x2);
            FileSystem.FileGet(1, ref MapHdr.Ori, Offset + 0x3);
            FileSystem.FileGet(1, ref MapHdr.LiqEHeight, Offset + 0x4);
            FileSystem.FileGet(1, ref MapHdr.LiqMode, Offset + 0x5);
            FileSystem.FileGet(1, ref MapHdr.LiqSpd, Offset + 0x6);
            FileSystem.FileGet(1, ref MapHdr.LiqSHeight, Offset + 0x7);
            FileSystem.FileGet(1, ref MapHdr.BorR, Offset + 0x8);
            FileSystem.FileGet(1, ref MapHdr.BorT, Offset + 0xC);
            FileSystem.FileGet(1, ref MapHdr.BorL, Offset + 0x10);
            FileSystem.FileGet(1, ref MapHdr.BorB, Offset + 0x14);
            FileSystem.FileGet(1, ref MapHdr.Flag, Offset + 0x18);
            FileSystem.FileGet(1, ref MapHdr.ObjCount, Offset + 0x1C);
            FileSystem.FileGet(1, ref MapHdr.SndCount, Offset + 0x20);
            FileSystem.FileGet(1, ref MapHdr.SnakeCount, Offset + 0x24);
            FileSystem.FileGet(1, ref MapHdr.ClearPipCount, Offset + 0x28);
            FileSystem.FileGet(1, ref MapHdr.CreeperCount, Offset + 0x2C);
            FileSystem.FileGet(1, ref MapHdr.iBlkCount, Offset + 0x30);
            FileSystem.FileGet(1, ref MapHdr.TrackBlkCount, Offset + 0x34);
            FileSystem.FileGet(1, ref MapHdr.GroundCount, Offset + 0x3C);
            FileSystem.FileGet(1, ref MapHdr.TrackCount, Offset + 0x40);
            FileSystem.FileGet(1, ref MapHdr.IceCount, Offset + 0x44);
            if (IO)
            {
                // ReDim ObjLocData(1, 300, 300), ObjInfo(1, 300, 300)
                // For i = 0 To 300
                // For M = 0 To 300
                // ObjLocData(0, i, M) = ""
                // ObjLocData(1, i, M) = ""

                // Next
                // Next
                ObjLocData = new ObjStr[2, 301, 301];
            }


            // 单位0x48  0x14500 (0x20 * 2600)Object
            MapObj = new MapObject[MapHdr.ObjCount];
            ObjLinkType = new byte[60001];
            var loopTo = (long)(MapHdr.ObjCount - 1);
            for (M = 0L; M <= loopTo; M++)
            {
                FileSystem.FileGet(1, ref MapObj[(int)M].X, Offset + 0x48 + 0x0 + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].Y, Offset + 0x48 + 0x4 + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].W, Offset + 0x48 + 0xA + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].H, Offset + 0x48 + 0xB + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].Flag, Offset + 0x48 + 0xC + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].CFlag, Offset + 0x48 + 0x10 + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].Ex, Offset + 0x48 + 0x14 + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].ID, Offset + 0x48 + 0x18 + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].CID, Offset + 0x48 + 0x1A + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].LID, Offset + 0x48 + 0x1C + M * 0x20L);
                FileSystem.FileGet(1, ref MapObj[(int)M].SID, Offset + 0x48 + 0x1E + M * 0x20L);
                MapObj[(int)M].LinkType = 0;
            }

            // 0x14584  0x4B0 (0x4 * 300)Sound Effect
            // 蛇砖块0x149F8  0x12D4 (0x3C4 * 5)Snake Block
            MapSnk = new MapSnakeBlock[MapHdr.SnakeCount];
            var loopTo1 = (long)(MapHdr.SnakeCount - 1);
            for (M = 0L; M <= loopTo1; M++)
            {
                FileSystem.FileGet(1, ref MapSnk[(int)M].index, Offset + 0x149F8 + 0x0 + M * 0x3C4L);
                FileSystem.FileGet(1, ref MapSnk[(int)M].NodeCount, Offset + 0x149F8 + 0x1 + M * 0x3C4L);
                MapSnk[(int)M].Node = new MapSnakeBlockNode[MapSnk[(int)M].NodeCount];
                var loopTo2 = (long)(MapSnk[(int)M].NodeCount - 1);
                for (i = 0L; i <= loopTo2; i++)
                {
                    FileSystem.FileGet(1, ref MapSnk[(int)M].Node[(int)i].index, Offset + 0x149F8 + 0x0 + M * 0x3C4L + i * 0x8L);
                    FileSystem.FileGet(1, ref MapSnk[(int)M].Node[(int)i].Dir, Offset + 0x149F8 + 0x6 + M * 0x3C4L + i * 0x8L);
                }
            }

            // 透明管0x15CCC  0xE420 (0x124 * 200)Clear Pipe
            MapCPipe = new MapClearPipe[MapHdr.ClearPipCount];
            var loopTo3 = (long)(MapHdr.ClearPipCount - 1);
            for (M = 0L; M <= loopTo3; M++)
            {
                FileSystem.FileGet(1, ref MapCPipe[(int)M].Index, Offset + 0x15CCC + 0x0 + M * 0x124L);
                FileSystem.FileGet(1, ref MapCPipe[(int)M].NodeCount, Offset + 0x15CCC + 0x1 + M * 0x124L);
                MapCPipe[(int)M].Node = new MapClearPipeNode[MapCPipe[(int)M].NodeCount];
                var loopTo4 = (long)(MapCPipe[(int)M].NodeCount - 1);
                for (i = 0L; i <= loopTo4; i++)
                {
                    FileSystem.FileGet(1, ref MapCPipe[(int)M].Node[(int)i].type, Offset + 0x15CCC + 0x4 + M * 0x124L + i * 0x8L);
                    FileSystem.FileGet(1, ref MapCPipe[(int)M].Node[(int)i].index, Offset + 0x15CCC + 0x5 + M * 0x124L + i * 0x8L);
                    FileSystem.FileGet(1, ref MapCPipe[(int)M].Node[(int)i].X, Offset + 0x15CCC + 0x6 + M * 0x124L + i * 0x8L);
                    FileSystem.FileGet(1, ref MapCPipe[(int)M].Node[(int)i].Y, Offset + 0x15CCC + 0x7 + M * 0x124L + i * 0x8L);
                    FileSystem.FileGet(1, ref MapCPipe[(int)M].Node[(int)i].W, Offset + 0x15CCC + 0x8 + M * 0x124L + i * 0x8L);
                    FileSystem.FileGet(1, ref MapCPipe[(int)M].Node[(int)i].H, Offset + 0x15CCC + 0x9 + M * 0x124L + i * 0x8L);
                    FileSystem.FileGet(1, ref MapCPipe[(int)M].Node[(int)i].Dir, Offset + 0x15CCC + 0xB + M * 0x124L + i * 0x8L);
                }
            }

            // 0x240EC  0x348 (0x54 * 10)Piranha Creeper
            MapCrp = new MapCreeper[MapHdr.CreeperCount];
            var loopTo5 = (long)(MapHdr.CreeperCount - 1);
            for (M = 0L; M <= loopTo5; M++)
            {
                FileSystem.FileGet(1, ref MapCrp[(int)M].index, Offset + 0x240EC + 0x1 + M * 0x54L);
                FileSystem.FileGet(1, ref MapCrp[(int)M].NodeCount, Offset + 0x240EC + 0x2 + M * 0x54L);
                MapCrp[(int)M].Node = new byte[MapCrp[(int)M].NodeCount];
                var loopTo6 = (long)(MapCrp[(int)M].NodeCount - 1);
                for (i = 0L; i <= loopTo6; i++)
                    FileSystem.FileGet(1, ref MapCrp[(int)M].Node[(int)i], Offset + 0x240EC + 0x4 + 0x1 + M * 0x54L + i * 0x4L);
            }

            // 0x24434  0x1B8 (0x2C * 10)! Block
            MapMoveBlk = new MapMoveBlock[MapHdr.iBlkCount];
            var loopTo7 = (long)(MapHdr.iBlkCount - 1);
            for (M = 0L; M <= loopTo7; M++)
            {
                FileSystem.FileGet(1, ref MapMoveBlk[(int)M].index, Offset + 0x24434 + 0x1 + M * 0x2CL);
                FileSystem.FileGet(1, ref MapMoveBlk[(int)M].NodeCount, Offset + 0x24434 + 0x2 + M * 0x2CL);
                MapMoveBlk[(int)M].Node = new MapMoveBlockNode[MapMoveBlk[(int)M].NodeCount];
                var loopTo8 = (long)(MapMoveBlk[(int)M].NodeCount - 1);
                for (i = 0L; i <= loopTo8; i++)
                {
                    FileSystem.FileGet(1, ref MapMoveBlk[(int)M].Node[(int)i].p0, Offset + 0x24434 + 0x4 + 0x0 + M * 0x2CL + i * 0x4L);
                    FileSystem.FileGet(1, ref MapMoveBlk[(int)M].Node[(int)i].p1, Offset + 0x24434 + 0x4 + 0x1 + M * 0x2CL + i * 0x4L);
                    FileSystem.FileGet(1, ref MapMoveBlk[(int)M].Node[(int)i].p2, Offset + 0x24434 + 0x4 + 0x2 + M * 0x2CL + i * 0x4L);
                }
            }

            // 0x245EC  0x1B8 (0x2C * 10)Track Block
            MapTrackBlk = new MapMoveBlock[MapHdr.TrackBlkCount];
            var loopTo9 = (long)(MapHdr.TrackBlkCount - 1);
            for (M = 0L; M <= loopTo9; M++)
            {
                FileSystem.FileGet(1, ref MapTrackBlk[(int)M].index, Offset + 0x245EC + 0x1 + M * 0x2CL);
                FileSystem.FileGet(1, ref MapTrackBlk[(int)M].NodeCount, Offset + 0x245EC + 0x2 + M * 0x2CL);
                MapTrackBlk[(int)M].Node = new MapMoveBlockNode[MapTrackBlk[(int)M].NodeCount];
                var loopTo10 = (long)(MapTrackBlk[(int)M].NodeCount - 1);
                for (i = 0L; i <= loopTo10; i++)
                {
                    FileSystem.FileGet(1, ref MapTrackBlk[(int)M].Node[(int)i].p0, Offset + 0x245EC + 0x4 + 0x0 + M * 0x2CL + i * 0x4L);
                    FileSystem.FileGet(1, ref MapTrackBlk[(int)M].Node[(int)i].p1, Offset + 0x245EC + 0x4 + 0x1 + M * 0x2CL + i * 0x4L);
                    FileSystem.FileGet(1, ref MapTrackBlk[(int)M].Node[(int)i].p2, Offset + 0x245EC + 0x4 + 0x2 + M * 0x2CL + i * 0x4L);
                }
            }

            // 地面0x247A4  0x3E80 (0x4 * 4000)Ground
            MapGrd = new MapGround[MapHdr.GroundCount];
            GroundNode = new int[301, 301];
            for (M = 0L; M <= 300L; M++)
            {
                for (j = 0L; j <= 300L; j++)
                    GroundNode[(int)M, (int)j] = 0;
            }

            var loopTo11 = (long)(MapHdr.GroundCount - 1);
            for (M = 0L; M <= loopTo11; M++)
            {
                FileSystem.FileGet(1, ref MapGrd[(int)M].X, Offset + 0x247A4 + 0x0 + M * 0x4L);
                FileSystem.FileGet(1, ref MapGrd[(int)M].Y, Offset + 0x247A4 + 0x1 + M * 0x4L);
                FileSystem.FileGet(1, ref MapGrd[(int)M].ID, Offset + 0x247A4 + 0x2 + M * 0x4L);
                FileSystem.FileGet(1, ref MapGrd[(int)M].BID, Offset + 0x247A4 + 0x3 + M * 0x4L);
                GroundNode[MapGrd[(int)M].X + 1, MapGrd[(int)M].Y + 1] = 1;
            }

            if (IO)
            {
                var loopTo12 = (long)Math.Round((LH.GoalX - 5) / 10d + 9d);
                for (j = (long)Math.Round((LH.GoalX - 5) / 10d); j <= loopTo12; j++)
                {
                    var loopTo13 = (long)(LH.GoalY - 1);
                    for (i = 0L; i <= loopTo13; i++)
                        GroundNode[(int)(j + 1L), (int)(i + 1L)] = 1;
                }

                for (j = 0L; j <= 6L; j++)
                {
                    var loopTo14 = (long)(LH.StartY - 1);
                    for (i = 0L; i <= loopTo14; i++)
                        GroundNode[(int)(j + 1L), (int)(i + 1L)] = 1;
                }
            }
            // 轨道0x28624  0x4650 (0xC * 1500)Track
            MapTrk = new MapTrack[MapHdr.TrackCount];
            TrackNode = new int[MapHdr.BorR + 3 + 1, MapHdr.BorT + 3 + 1];
            var TX = default(byte);
            var loopTo15 = (long)(MapHdr.TrackCount - 1);
            for (M = 0L; M <= loopTo15; M++)
            {
                FileSystem.FileGet(1, ref MapTrk[(int)M].UN, Offset + 0x28624 + 0x0 + M * 0xCL);
                FileSystem.FileGet(1, ref MapTrk[(int)M].Flag, Offset + 0x28624 + 0x2 + M * 0xCL);
                FileSystem.FileGet(1, ref TX, Offset + 0x28624 + 0x3 + M * 0xCL);
                if (TX == 255)
                {
                    MapTrk[(int)M].X = 0;
                }
                else
                {
                    MapTrk[(int)M].X = (byte)(TX + 1);
                }

                FileSystem.FileGet(1, ref TX, Offset + 0x28624 + 0x4 + M * 0xCL);
                if (TX == 255)
                {
                    MapTrk[(int)M].Y = 0;
                }
                else
                {
                    MapTrk[(int)M].Y = (byte)(TX + 1);
                }

                FileSystem.FileGet(1, ref MapTrk[(int)M].Type, Offset + 0x28624 + 0x5 + M * 0xCL);
                FileSystem.FileGet(1, ref MapTrk[(int)M].LID, Offset + 0x28624 + 0x6 + M * 0xCL);
                FileSystem.FileGet(1, ref MapTrk[(int)M].K0, Offset + 0x28624 + 0x8 + M * 0xCL);
                FileSystem.FileGet(1, ref MapTrk[(int)M].K1, Offset + 0x28624 + 0xA + M * 0xCL);
                // MapTrk(M).K0 = MapTrk(M).K0 >> 16
                // MapTrk(M).K1 = MapTrk(M).K1 >> 16
                switch (MapTrk[(int)M].Type)
                {
                    case 0:
                        {
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y + 1] += 1;
                            TrackNode[MapTrk[(int)M].X + 2, MapTrk[(int)M].Y + 1] += 1;
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K1 >> 7) % 2);
                            break;
                        }

                    case 1:
                        {
                            TrackNode[MapTrk[(int)M].X + 1, MapTrk[(int)M].Y + 2] += 1;
                            TrackNode[MapTrk[(int)M].X + 1, MapTrk[(int)M].Y] += 1;
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K1 >> 7) % 2);
                            break;
                        }

                    case 2:
                    case 4:
                    case 5:
                        {
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y + 2] += 1;
                            TrackNode[MapTrk[(int)M].X + 2, MapTrk[(int)M].Y] += 1;
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K1 >> 7) % 2);
                            break;
                        }

                    case 3:
                    case 6:
                    case 7:
                        {
                            TrackNode[MapTrk[(int)M].X + 2, MapTrk[(int)M].Y + 2] += 1;
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y] += 1;
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K1 >> 7) % 2);
                            break;
                        }

                    case 8:
                        {
                            // F0标记
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K1 >> 6) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            MapTrk[(int)M].F2 = (byte)(((MapTrk[(int)M].K0 >> 15) + 1) % 2);
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y + 2] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y + 4] += 1;
                            break;
                        }

                    case 9:
                        {
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K1 >> 6) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K1 >> 1) % 2);
                            MapTrk[(int)M].F2 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y + 4] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y + 2] += 1;
                            break;
                        }

                    case 10:
                        {
                            MapTrk[(int)M].F0 = (byte)(((MapTrk[(int)M].K0 >> 14) + 1) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K1 >> 6) % 2);
                            MapTrk[(int)M].F2 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X + 2, MapTrk[(int)M].Y + 4] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y] += 1;
                            break;
                        }

                    case 11:
                        {
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K1 >> 1) % 2);
                            MapTrk[(int)M].F2 = (byte)((MapTrk[(int)M].K1 >> 6) % 2);
                            TrackNode[MapTrk[(int)M].X + 2, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y + 4] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y + 4] += 1;
                            break;
                        }

                    case 12:
                        {
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K1 >> 11) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            MapTrk[(int)M].F2 = (byte)((MapTrk[(int)M].K0 >> 12) % 2);
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y + 2] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y + 4] += 1;
                            break;
                        }

                    case 13:
                        {
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K1 >> 11) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K0 >> 12) % 2);
                            MapTrk[(int)M].F2 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y + 4] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y + 2] += 1;
                            break;
                        }

                    case 14:
                        {
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K0 >> 12) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K1 >> 11) % 2);
                            MapTrk[(int)M].F2 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X + 2, MapTrk[(int)M].Y + 4] += 1;
                            break;
                        }

                    case 15:
                        {
                            MapTrk[(int)M].F0 = (byte)((MapTrk[(int)M].K0 >> 7) % 2);
                            MapTrk[(int)M].F1 = (byte)((MapTrk[(int)M].K0 >> 12) % 2);
                            MapTrk[(int)M].F2 = (byte)((MapTrk[(int)M].K1 >> 11) % 2);
                            TrackNode[MapTrk[(int)M].X + 2, MapTrk[(int)M].Y] += 1;
                            TrackNode[MapTrk[(int)M].X, MapTrk[(int)M].Y + 4] += 1;
                            TrackNode[MapTrk[(int)M].X + 4, MapTrk[(int)M].Y + 4] += 1;
                            break;
                        }
                }
            }
            // 冰块0x2CC74  0x4B0 (0x4 * 300)Icicle
            MapIce = new MapGround[MapHdr.IceCount];
            var loopTo16 = (long)(MapHdr.IceCount - 1);
            for (M = 0L; M <= loopTo16; M++)
            {
                FileSystem.FileGet(1, ref MapIce[(int)M].X, Offset + 0x2CC74 + 0x0 + M * 0x4L);
                FileSystem.FileGet(1, ref MapIce[(int)M].Y, Offset + 0x2CC74 + 0x1 + M * 0x4L);
                FileSystem.FileGet(1, ref MapIce[(int)M].ID, Offset + 0x2CC74 + 0x2 + M * 0x4L);
            }

            FileSystem.FileClose(1);
        }

        public struct ObjStr
        {
            public string Obj;
            public string Flag;
            public string State;
            public string SubObj;
            public string SubFlag;
            public string SubState;
        }

        public static ObjStr[,,] ObjLocData;

        public static Image GetItemImg(ObjStr Obj, ref int W, ref int H)
        {
            Image GetItemImgRet = default;
            string[] S0, S1, S2, S3, S4, S5;
            if (Information.IsNothing(Obj.Obj))
            {
                W = 0;
                H = 0;
                GetItemImgRet = null;
            }
            else
            {
                S0 = Obj.Obj.Split(',');
                S1 = Obj.Flag.Split(',');
                S2 = Obj.State.Split(',');
                S3 = Obj.SubObj.Split(',');
                S4 = Obj.SubFlag.Split(',');
                S5 = Obj.SubState.Split(',');
                H = S0.GetUpperBound(0) * (64 + 6) - 6;
                W = 64 * 2 + 32;
                var BB = new Bitmap(W, H);
                var GG = Graphics.FromImage(BB);
                // GG.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                int I, J;
                var loopTo = S0.GetUpperBound(0) - 1;
                for (I = 0; I <= loopTo; I++)
                {
                    if (S0[I].Length > 0)
                    {
                        GG.DrawImage(Image.FromFile(PT + @"\IMG\" + LH.GameStyle.ToString() + @"\DLY\" + S0[I] + S2[I] + ".PNG"), 0, I * 70, 64, 64);
                        var loopTo1 = S1[I].Length;
                        for (J = 1; J <= loopTo1; J++)
                        {
                            GG.DrawImage(SetOpacity((Bitmap)Image.FromFile(PT + @"\IMG\CMN\F0.PNG"), 1d), J * 24 - 20, I * 70 + 70 - 32, 24, 24);
                            GG.DrawImage(SetOpacity((Bitmap)Image.FromFile(PT + @"\IMG\" + LH.GameStyle.ToString() + @"\J" + Strings.Mid(S1[I], J, 1) + ".PNG"), 1d), J * 24 - 20, I * 70 + 70 - 32, 24, 24);
                        }

                        if (S3[I].Length > 0)
                        {
                            GG.DrawImage(Image.FromFile(PT + @"\IMG\CMN\G0.PNG"), 64, I * 70 + 16, 32, 32);
                            GG.DrawImage(Image.FromFile(PT + @"\IMG\" + LH.GameStyle.ToString() + @"\DLY\" + S3[I] + S5[I] + ".PNG"), 88, I * 70, 64, 64);
                            var loopTo2 = S4[I].Length;
                            for (J = 1; J <= loopTo2; J++)
                            {
                                GG.DrawImage(SetOpacity((Bitmap)Image.FromFile(PT + @"\IMG\CMN\F0.PNG"), 1d), 88 - 20 + J * 24, I * 70 + 70 - 32, 24, 24);
                                GG.DrawImage(SetOpacity((Bitmap)Image.FromFile(PT + @"\IMG\" + LH.GameStyle.ToString() + @"\J" + Strings.Mid(S4[I], J, 1) + ".PNG"), 1d), 88 - 20 + J * 24, I * 70 + 70 - 32, 24, 24);
                            }
                        }
                    }
                }

                GetItemImgRet = BB;
                My.MyProject.Forms.Form1.PicBot.Image = BB;
            }

            return GetItemImgRet;
        }

        public static Bitmap SetOpacity(Bitmap B, double D)
        {
            try
            {
                var bmpDATA = new System.Drawing.Imaging.BitmapData();
                var tmpBMP = new Bitmap(B);
                var Rct = new Rectangle(0, 0, B.Width, B.Height);
                bmpDATA = tmpBMP.LockBits(Rct, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var BTS = new byte[bmpDATA.Stride * bmpDATA.Height + 1];
                System.Runtime.InteropServices.Marshal.Copy(bmpDATA.Scan0, BTS, 0, BTS.Length - 1);
                double T = 0d;
                for (int I = 0, loopTo = BTS.Length - 4; I <= loopTo; I += 4)
                {
                    T = BTS[I + 3];
                    T = T * D;
                    BTS[I + 3] = (byte)Math.Round(T);
                }

                System.Runtime.InteropServices.Marshal.Copy(BTS, 0, bmpDATA.Scan0, BTS.Length - 1);
                tmpBMP.UnlockBits(bmpDATA);
                return tmpBMP;
            }
            catch
            {
                return null;
            }
        }
    }
}