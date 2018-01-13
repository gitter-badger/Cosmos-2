﻿using System;
using Cosmos.Logging.Events;
using Cosmos.Logging.RunsOn.Console;
using Cosmos.Logging.RunsOn.Console.Settings;
using Cosmos.Logging.Sinks.NLog;
using Cosmos.Logging.Sinks.SampleLogSink;

namespace Cosmos.Logging.CollaborativeTesting.NLogAndSampleLog {
    class Program {
        static void Main(string[] args) {
            try {
                //var config = GetNLogConfig();

                LOGGER.Initialize().RunsOnConsole()
                    .UseNLog(s => s.EnableUsingDefaultConfig())
                    .UseSampleLog(s => {
                        s.Name = "sampleSink_123";
                        s.Level = LogEventLevel.Debug;
                    })
                    .AllDone();

                var logger = LOGGER.GetLogger(mode: LogEventSendMode.Manually);

                logger.Information("hello");
                logger.Error("world");
                logger.SubmitLogger();

                var logger2 = LOGGER.GetLogger("sampleSink_123222");
                logger2.Information("logger2 ---> {{alexLEWIS} }.}}..{}.{{}}");  

                Console.WriteLine("Hello World!");
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }

            Console.ReadLine();
        }
    }
}