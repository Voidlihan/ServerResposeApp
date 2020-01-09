﻿using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ServerResposeApp
{
    class Program
    {
        static async void Main(string[] args)
        {
            string json = @"
                {
                'Email': 'james@example.com',
                'Active': true,
                'CreatedDate': '2013-01-20T00:00:00Z',
                'Roles': [
                'User',
                'Admin'
                ]
                }";
            var listener = new TcpListener(IPAddress.Parse("10.1.4.82"), 3231);
            listener.Start();
            Console.WriteLine("Сервер запущен");
            while (true)
            {
                using (var client = listener.AcceptTcpClient())
                {
                    Console.WriteLine("Входящее соединение");
                    using (var stream = client.GetStream())
                    {
                        var resText = String.Empty;
                        while (stream.DataAvailable)
                        {
                            var buffer = new byte[1024];
                            stream.Read(buffer, 0, buffer.Length);
                            resText += System.Text.Encoding.UTF8.GetString(buffer);
                        }
                        var obj = JObject.Parse(json);
                        if (String.Equals((string)obj["Path"], "/user/"))
                        {
                            var data = JsonConvert.DeserializeObject<Request<User>>(json);
                            using (var context = new Context())
                            {
                                switch (data.Action)
                                {
                                    case "Give": 
                                                                               
                                        break;
                                    case "Add":
                                        context.Add<User>(data.Add);
                                        break;
                                    case "Update":                                         
                                        context.Add<User>(data.Update);
                                        break;
                                    case "Remove": 
                                        context.Add<User>(data.Remove);
                                        break;
                                }
                                await context.SaveChangesAsync();
                            }
                        }
                        Console.WriteLine($"Данные от клиента - {resText}");
                        var answerData = System.Text.Encoding.UTF8.GetBytes("Запрос получен");
                        stream.Write(answerData, 0, answerData.Length);
                    }
                }
                Console.WriteLine("Соединение закрыто");
            }
        }

    }
}