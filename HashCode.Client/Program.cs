namespace HashCode.Client
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Write.StartWatch();
            Write.TraceVisible("example", true);

            using (Round round = Round.RoundFromFile(Inputs.InExample))
            {         
                round.AssignVideos(Round.AssignMode.Standard);
                round.PrintAssigment(Inputs.OutExample);
            }

            Write.TraceWatch("process time", true, true);
            Write.TraceVisible("me_at_the_zoo", true);

            using (Round round = Round.RoundFromFile(Inputs.InMeAtTheZoo))
            {
                round.AssignVideos(Round.AssignMode.Standard);
                round.PrintAssigment(Inputs.OutMeAtTheZoo);
            }

            Write.TraceWatch("process time", true, true);
            Write.TraceVisible("videos_worth_spreading");

            using (Round round = Round.RoundFromFile(Inputs.InVideosWorthSpreading))
            {
                round.AssignVideos(Round.AssignMode.Standard);
                round.PrintAssigment(Inputs.OutVideosWorthSpreading);
            }

            Write.TraceWatch("process time", true, true);
            Write.TraceVisible("trending_today", true);

            using (Round round = Round.RoundFromFile(Inputs.InTrendingToday))
            {
                round.AssignVideos(Round.AssignMode.Standard);
                round.PrintAssigment(Inputs.OutTrendingToday);
            }

            Write.TraceWatch("process time", true, true);

            Write.TraceVisible("kitten", true);
            
            using (Round round = Round.RoundFromFile(Inputs.InKitten))
            {
                round.AssignVideos(Round.AssignMode.PreProcessing);
                round.PrintAssigment(Inputs.OutKitten);
            }

            Write.TraceWatch("boom boom boom and everybody say HEyyHOOOooO", true);

            Console.ReadKey();
        }
    }
}