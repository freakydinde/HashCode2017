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
                round.AssignVideos();
                round.PrintAssigment(Inputs.OutExample);
                if (round.ComputeScore(Inputs.OutExample)) round.TraceScore();
            }

            Write.TraceVisible("me_at_the_zoo", true);

            using (Round round = Round.RoundFromFile(Inputs.InMeAtTheZoo))
            {
                round.AssignVideos();
                round.PrintAssigment(Inputs.OutMeAtTheZoo);
                if (round.ComputeScore(Inputs.OutMeAtTheZoo)) round.TraceScore();
            }

            Write.TraceVisible("videos_worth_spreading", true);

            using (Round round = Round.RoundFromFile(Inputs.InVideosWorthSpreading))
            {
                round.AssignVideos();
                round.PrintAssigment(Inputs.OutVideosWorthSpreading);
                if (round.ComputeScore(Inputs.OutVideosWorthSpreading)) round.TraceScore();
            }

            Write.TraceVisible("trending_today", true);

            using (Round round = Round.RoundFromFile(Inputs.InTrendingToday))
            {
                round.AssignVideos();
                round.PrintAssigment(Inputs.OutTrendingToday);
                if (round.ComputeScore(Inputs.OutTrendingToday)) round.TraceScore();
            }

            Write.TraceVisible("kitten", true);
            
            using (Round round = Round.RoundFromFile(Inputs.InKitten))
            {
                round.AssignVideos();
                round.PrintAssigment(Inputs.OutKitten);
                if (round.ComputeScore(Inputs.OutKitten)) round.TraceScore();
            }

            Write.TraceWatch("boom boom boom and everybody say HEyyHOOOooO", true);

            Console.ReadKey();
        }
    }
}