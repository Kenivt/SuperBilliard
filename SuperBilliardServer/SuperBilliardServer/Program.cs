using SuperBilliardServer;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Debug.MessageStyle;

public class Program
{
    private static bool _serverRunning = true;

    static void Main()
    {
        ServerBase.Init();
        CommandParser();
        while (_serverRunning)
        {
            Thread.Sleep(1);
            ServerBase.Update();
        }
        ServerBase.ShutDown();
    }

    private static async void CommandParser()
    {
        while (_serverRunning)
        {
            string? command = await Task.Run(() => Console.ReadLine()?.ToLower());
            try
            {
                if (command == "quit")
                {
                    _serverRunning = false;
                    Log.Info("退出程序...");
                }
                else if (command == "display refpool")
                {
                    Log.ShowStyle<ShowReferenceInfo>();
                }
                else if (command == "clear")
                {
                    Console.Clear();
                }
                else if (command == "display gameroom")
                {
                    int count = 0;
                    foreach (var tuple in GameManager.Instance.GetAllGameRoomInfo())
                    {
                        count++;
                        Console.WriteLine("游玩模式：{0}", tuple.Item1.ToString());
                        foreach (var item in tuple.Item2)
                        {
                            Log.ShowStyle<GameInfoStyle>("玩家数量：", item.playerCount.ToString(), "房间状态:",
                            item.gameState.ToString(), "玩家1:", item.playerName1, "玩家2:", item.playerName2);
                        }
                    }
                    if (count == 0) Console.WriteLine("没有玩家处于匹配中...");
                }
                else if (command == "GC")
                {
                    GC.Collect();
                }
                else
                {
                    Log.Warning("没有输入任何命令...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {command},Message{1}", ex.ToString());
            }
        }
    }
}