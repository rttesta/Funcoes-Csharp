using System;
using System.Globalization;
using System.Threading;
using System.Timers;

namespace ConsoleApp1
{
    class Program
    {
        static System.Timers.Timer timer = new System.Timers.Timer();
        static bool fim = false;

        static void Main(string[] args)
        {
            TimeSpan result = TimeSpan.Zero;

            //result = NextRun("05:00", "22:00", "00:10", "");

            //Console.WriteLine(result.TotalSeconds > 0 ? result.ToString(@"hh\:mm\:ss") : "0");

            ////Console.ReadKey();

            //result = NextRun("08:00", "", "", "");

            //Console.WriteLine(result.TotalSeconds > 0 ? result.ToString(@"hh\:mm\:ss") : "0");

            ////Console.ReadKey();

            //result = NextRun("04:30", "", "", "22/12/2019");

            //Console.WriteLine(result.TotalSeconds > 0 ? result.ToString(@"hh\:mm\:ss") : "0");

            ////Console.ReadKey();

            //result = NextRun("19:30", "", "", "10/12/2019");

            //Console.WriteLine(result.TotalSeconds > 0 ? result.ToString(@"hh\:mm\:ss") : "0");

            ////Console.ReadKey();

            //result = NextRun("19:30", "", "00:30", "");

            //Console.WriteLine(result.TotalSeconds > 0 ? result.ToString(@"hh\:mm\:ss") : "0");

            //Console.ReadKey();

            result = NextRun("11:00", "", "00:01", "");

            Console.WriteLine("Proxima execução = " + result.ToString(@"hh\:mm\:ss") + " - " + DateTime.Now.AddSeconds(result.TotalSeconds).ToString());

            timer.Elapsed += new ElapsedEventHandler(TesteNextRun);

            timer.Interval = result.TotalMilliseconds;

            timer.Enabled = true;
            
            while (fim == false)
            {
                Thread.Sleep(1000);
            }

        }

        public static void TesteNextRun(object source, ElapsedEventArgs e)
        {
            timer.Enabled = false;

            TimeSpan result = NextRun("05:00", "", "00:10", "14/12/2019");

            if (result != TimeSpan.Zero)
            {
                Console.WriteLine("Proxima execução = " + result.ToString(@"hh\:mm\:ss") + " - " + DateTime.Now.AddSeconds(result.TotalSeconds).ToString());
                timer.Interval = result.TotalMilliseconds;
                timer.Enabled = true;
            }
            else
            {
                fim = true;
            }
        }

        public static TimeSpan NextRun(string HoraInicial, string HoraFinal, string Repeticao, string DataLimite)
        {
            double horaCalculada = 0;

            try
            {
                DateTime dataAtual = DateTime.Now;

                Console.WriteLine("Agora = " + dataAtual.ToString(@"hh\:mm\:ss"));

                TimeSpan horaInicial = TimeSpan.Zero;
                if (!TimeSpan.TryParseExact(HoraInicial, @"h\:m", CultureInfo.InvariantCulture, out horaInicial))
                {
                    return TimeSpan.Zero;
                }
                TimeSpan horaFinal = TimeSpan.Zero;
                if (!string.IsNullOrEmpty(HoraFinal))
                {
                    if (!TimeSpan.TryParseExact(HoraFinal, @"h\:m", CultureInfo.InvariantCulture, out horaFinal))
                    {
                        return TimeSpan.Zero;
                    }
                }
                TimeSpan repeticao = TimeSpan.Zero;
                if (!string.IsNullOrEmpty(Repeticao))
                {
                    if (!TimeSpan.TryParseExact(Repeticao, @"h\:m", CultureInfo.InvariantCulture, out repeticao))
                    {
                        return TimeSpan.Zero;
                    }
                }
                DateTime datalimite = DateTime.MinValue;
                if (!string.IsNullOrEmpty(DataLimite))
                {
                    if (DateTime.TryParseExact(DataLimite, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datalimite))
                    {
                        if (dataAtual > datalimite)
                        {
                            return TimeSpan.Zero;
                        }
                    }
                }

                TimeSpan hrAtual = new TimeSpan(dataAtual.Hour, dataAtual.Minute, dataAtual.Second);
                if (hrAtual < horaInicial)
                {
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horaInicial.Hours, horaInicial.Minutes, 0);
                    horaCalculada = dt.Subtract(DateTime.Now).TotalSeconds;
                }
                else
                {
                    if (repeticao > TimeSpan.Zero)
                    {
                        TimeSpan hrCalculada = hrAtual.Add(repeticao);
                        if ((horaFinal > TimeSpan.Zero) && (hrCalculada > horaFinal))
                        {
                            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horaInicial.Hours, horaInicial.Minutes, 0);
                            dt = dt.AddDays(1);
                            horaCalculada = dt.Subtract(DateTime.Now).TotalSeconds;
                        }
                        else
                        {
                            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hrCalculada.Hours, hrCalculada.Minutes, hrCalculada.Seconds, hrCalculada.Milliseconds);
                            horaCalculada = dt.Subtract(DateTime.Now).TotalSeconds;
                        }
                    }
                    else
                    {
                        DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horaInicial.Hours, horaInicial.Minutes, 0);
                        dt = dt.AddDays(1);
                        horaCalculada = dt.Subtract(DateTime.Now).TotalSeconds;
                    }
                }
               
                if (datalimite != DateTime.MinValue)
                {                   
                    if (horaCalculada > datalimite.Subtract(dataAtual).TotalSeconds)
                    {
                        return TimeSpan.Zero;
                    }
                }

                TimeSpan tsRetorno = new TimeSpan(0, 0, (int)horaCalculada);

                return tsRetorno;

            }
            catch
            {
                return TimeSpan.Zero;
            }
        }
    }
}
