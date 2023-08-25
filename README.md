##  基于GameFrameWork框架的Unity台球游戏

---

​		本项目是基于GameFrameWork框架的一个台球游戏,使用了C#做服务端，Sqlserver做数据库。是本人对GF框架的一次深入学习实践的结果。

### 客户端

#### 游玩模式

​		本项目包含两种台球模式，花式台球和斯诺克台球两种模式。其中花式台球是最早制作的模式，当时并没有理清台球同步具体逻辑，所以其Rule类写的有些杂乱，同步了多余的消息。而在斯诺克台球的同步逻辑中，遵循着同步台球位置信息和同步回合结束消息的规则，其余逻辑基于同步的消息进行分析判断。

#### 匹配模式

1. 普通匹配,通过点击匹配会在服务器中寻找相应的玩家进行对战。
2. 好友联机,通过添加好友和创建好友房间来进行好友对战。

#### 游戏逻辑

在ProcedureLevel流程中,通过反射创建对应的IBattle类，进而来控制游玩不同的模式。

IBattle类中有如下组成：

			+ IBattleRuleSystem 通过事件订阅进球事件和回合结束事件来进行回合得分分析。
			+ IBattleController 通过状态机来模拟一个回合中各个游戏的状态。
			+ IBattleBuilder 通过此类来加载一些物品。
			+ IBattleData 通过此类来记录游戏过程中数据进而使IBattleRuleSystem进行分析。

#### 网络同步

​		服务器来充当转发消息的作用。其中使用了KCP同步台球的位置,TCP同步一些诸如游戏开始,加载完成,回合结束的通知。

​		在做这个项目的过程中做了很多无用功,比如最开始构思台球同步逻辑时想要使用一个开源的定点数引擎，想要只同步玩家的操作，让物理引擎获得相同的结果，可是同步的结果不尽人意，进而使用实时同步台球位置的方案。

### 服务端

​		使用了C#来做服务端,Sqlserver来做数据库。

​		整体的架构，模仿了ET的单例模式，通过多个单例类来统一的进行管理。

  1. ClientManager用来缓存各个客户端，轮询所有客户端检测客户端的连接情况,并发送确认包。

  2. SqlManager统一的对数据库进行读取操作。

     ~~~ c#
     //简易的类IOC,方便进行数据库的迁移
     SqlManager.Instance.RigisterSqlHandler<IFriendSqlHandler>(new FriendSqlHandler());
     SqlManager.Instance.RigisterSqlHandler<ILoginSqlHandler>(new LogicSqlHandler());
     SqlManager.Instance.RigisterSqlHandler<IPlayerMessageSqlHandler>(new PlayerMessageSqlHandler());
     //获取连接
     SqlManager.Instance.GetConnection();
     ~~~

  3. PacketManager通过类似GF的NetWorkManager来统一管理和解析消息包。

### 致谢仓库

[GameFramework](https://github.com/EllanJiang/GameFramework)

[TowerDefense-GameFramework-Demo: 基于Unity开源框架GameFramewrk实现的一款塔防游戏Demo (github.com)](https://github.com/DrFlower/TowerDefense-GameFramework-Demo)

[KCP_Csharp](https://github.com/KumoKyaku/KCP)
