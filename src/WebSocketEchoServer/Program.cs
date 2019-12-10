using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using Fleck;
using Console = Colorful.Console;

namespace WebSocketEchoServer
{
    class Program
    {
        private static WebSocketServer _wssv;
        private static IList<IWebSocketConnection> _wsconnections;

        static void Main(string[] args)
        {
            _wssv = new WebSocketServer(ConfigurationManager.AppSettings["WSUrl"]);

            _wsconnections = new List<IWebSocketConnection>();

            FleckLog.LogAction = (level, message, ex) =>
            {
                Color color;

                switch (level)
                {
                    case LogLevel.Info:
                        color = Color.White;
                        break;
                    case LogLevel.Debug:
                        color = Color.LightSkyBlue;
                        break;
                    case LogLevel.Error:
                        color = Color.OrangeRed;
                        break;
                    case LogLevel.Warn:
                        color = Color.Orange;
                        break;
                    default:
                        color = Color.White;
                        break;
                }

                Console.WriteLine($"{DateTime.Now}[{level}] {message} {ex}", color);
            };
            
            _wssv.Start(socket =>
            {
                socket.OnOpen = () => {
                    Console.WriteLine($"[OnOpen] {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} connected", Color.SeaGreen);
                    _wsconnections.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine($"[OnClose] {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} connection closed", Color.OrangeRed);
                    _wsconnections.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine($"[OnMessage] {message.ToString()}", Color.BurlyWood);
                };
            });

            Console.WriteLine("press Enter to exit", Color.White);
            Console.ReadLine();
            
            foreach (var socket in _wsconnections)
            {
                socket.Close();
            }

            Console.WriteLine($"Closing {_wsconnections.Count} client connections and disposing of ws server, Exiting...", Color.White);

            _wssv.Dispose();
        }
    }
}
