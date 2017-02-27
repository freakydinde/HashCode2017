namespace HashCode.Client
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Write.Print("example");

            Round round = Round.RoundFromFile(Inputs.InExample);

            round.AssignVideos();
            round.PrintAssigment(Inputs.OutExample);

            Write.PrintWatch();
            Write.ResetWatch();

            Write.Print("me_at_the_zoo");

            round = Round.RoundFromFile(Inputs.InMeAtTheZoo);

            round.AssignVideos();
            round.PrintAssigment(Inputs.OutMeAtTheZoo);

            Write.PrintWatch();
            Write.ResetWatch();

            Write.Print("videos_worth_spreading");

            round = Round.RoundFromFile(Inputs.InVideosWorthSpreading);

            round.AssignVideos();
            round.PrintAssigment(Inputs.OutVideosWorthSpreading);

            Write.PrintWatch();
            Write.ResetWatch();

            Write.Print("trending_today");

            round = Round.RoundFromFile(Inputs.InTrendingToday);

            round.AssignVideos();
            round.PrintAssigment(Inputs.OutTrendingToday);

            Write.PrintWatch();
            Write.ResetWatch();

            Write.Print("kitten");

            round = Round.RoundFromFile(Inputs.InKitten);

            round.AssignVideos();
            round.PrintAssigment(Inputs.OutKitten);

            Write.PrintWatch();
            Write.ResetWatch();

            Write.Print("boom boom boom and everybody say HEHOOOOOOO");

            Console.ReadKey();
        }
    }
}