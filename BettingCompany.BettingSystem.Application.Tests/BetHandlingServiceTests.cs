using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BettingCompany.BettingSystem.Application.Tests
{
    public class BetHandlingServiceTests
    {
        Stopwatch stopwatch = new Stopwatch();

        [Fact]
        public async Task HandleAsync_HandleOneHundreadIncomingBets_CorrectResults()
        {
            var mockRepository = new Mock<IBetRepository>();

            var savedBets = new List<BetCalculated>();

            mockRepository
                .Setup(repo => repo.Save(It.IsAny<IList<BetCalculated>>()))
                .Callback<IList<BetCalculated>>(b =>
                {
                    savedBets.AddRange(b);
                    
                });

            var betHandlingService = new BetHandlingService(
                new BetAgregator(),
                new WorkersDirector(maxWorkers: 50, new WorkersFactory()),
                new PersistancePolicy(),
                new DateTimeProvider(), // todo: mock date time provider
                mockRepository.Object);


            var testBets = new List<Bet>
{
    new Bet(1, 50.45, 2.57, "client_4", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:02Z") },
    new Bet(1, 50.45, 2.57, "client_4", "event_3", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:04Z") },
    new Bet(2, 81.16, 2.3, "client_2", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:04Z") },
    new Bet(2, 81.16, 2.3, "client_2", "event_2", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:05Z") },
    new Bet(3, 63.31, 1.72, "client_1", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:06Z") },
    new Bet(3, 63.31, 1.72, "client_1", "event_3", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:09Z") },
    new Bet(4, 34.2, 2.53, "client_2", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:08Z") },
    new Bet(4, 34.2, 2.53, "client_2", "event_2", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:09Z") },
    new Bet(5, 93.79, 2.35, "client_4", "event_10", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:10Z") },
    new Bet(5, 93.79, 2.35, "client_4", "event_10", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:12Z") },
    new Bet(6, 93.05, 1.91, "client_1", "event_8", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:12Z") },
    new Bet(6, 93.05, 1.91, "client_1", "event_8", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:13Z") },
    new Bet(7, 21.18, 1.6, "client_3", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:14Z") },
    new Bet(7, 21.18, 1.6, "client_3", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:17Z") },
    new Bet(8, 48.39, 2.16, "client_5", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:16Z") },
    new Bet(8, 48.39, 2.16, "client_5", "event_2", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:19Z") },
    new Bet(9, 80.83, 2.42, "client_4", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:18Z") },
    new Bet(9, 80.83, 2.42, "client_4", "event_1", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:19Z") },
    new Bet(10, 36.1, 2.29, "client_5", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:20Z") },
    new Bet(10, 36.1, 2.29, "client_5", "event_3", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:21Z") },
    new Bet(11, 56.04, 2.93, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:22Z") },
    new Bet(11, 56.04, 2.93, "client_2", "event_1", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:25Z") },
    new Bet(12, 19.46, 1.87, "client_2", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:24Z") },
    new Bet(12, 19.46, 1.87, "client_2", "event_5", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:25Z") },
    new Bet(13, 67.51, 2.2, "client_1", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:26Z") },
    new Bet(13, 67.51, 2.2, "client_1", "event_9", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:27Z") },
    new Bet(14, 13.12, 2.3, "client_2", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:28Z") },
    new Bet(14, 13.12, 2.3, "client_2", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:30Z") },
    new Bet(15, 35.57, 2.88, "client_3", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:30Z") },
    new Bet(15, 35.57, 2.88, "client_3", "event_5", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:32Z") },
    new Bet(16, 23.62, 1.87, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:32Z") },
    new Bet(16, 23.62, 1.87, "client_2", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:34Z") },
    new Bet(17, 62.65, 2.5, "client_2", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:34Z") },
    new Bet(17, 62.65, 2.5, "client_2", "event_2", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:37Z") },
    new Bet(18, 25.53, 1.71, "client_3", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:36Z") },
    new Bet(18, 25.53, 1.71, "client_3", "event_9", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:38Z") },
    new Bet(19, 82.0, 1.91, "client_3", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:38Z") },
    new Bet(19, 82.0, 1.91, "client_3", "event_7", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:40Z") },
    new Bet(20, 70.88, 1.83, "client_3", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:40Z") },
    new Bet(20, 70.88, 1.83, "client_3", "event_9", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:42Z") },
    new Bet(21, 47.22, 2.45, "client_1", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:42Z") },
    new Bet(21, 47.22, 2.45, "client_1", "event_10", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:43Z") },
    new Bet(22, 42.08, 2.34, "client_3", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:44Z") },
    new Bet(22, 42.08, 2.34, "client_3", "event_7", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:46Z") },
    new Bet(23, 37.14, 2.99, "client_4", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:46Z") },
    new Bet(23, 37.14, 2.99, "client_4", "event_9", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:47Z") },
    new Bet(24, 52.99, 1.63, "client_5", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:48Z") },
    new Bet(24, 52.99, 1.63, "client_5", "event_3", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:49Z") },
    new Bet(25, 45.15, 2.0, "client_4", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:50Z") },
    new Bet(25, 45.15, 2.0, "client_4", "event_5", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:53Z") },
    new Bet(26, 91.66, 2.62, "client_4", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:52Z") },
    new Bet(26, 91.66, 2.62, "client_4", "event_3", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:54Z") },
    new Bet(27, 24.79, 2.92, "client_1", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:54Z") },
    new Bet(27, 24.79, 2.92, "client_1", "event_1", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:55Z") },
    new Bet(28, 50.84, 2.61, "client_3", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:56Z") },
    new Bet(28, 50.84, 2.61, "client_3", "event_10", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:58Z") },
    new Bet(29, 73.43, 2.92, "client_4", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:58Z") },
    new Bet(29, 73.43, 2.92, "client_4", "event_4", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:59Z") },
    new Bet(30, 10.12, 1.76, "client_3", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:00Z") },
    new Bet(30, 10.12, 1.76, "client_3", "event_4", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:02Z") },
    new Bet(31, 53.82, 2.78, "client_4", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:02Z") },
    new Bet(31, 53.82, 2.78, "client_4", "event_2", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:03Z") },
    new Bet(32, 31.1, 1.8, "client_4", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:04Z") },
    new Bet(32, 31.1, 1.8, "client_4", "event_7", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:07Z") },
    new Bet(33, 30.94, 2.29, "client_1", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:06Z") },
    new Bet(33, 30.94, 2.29, "client_1", "event_4", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:08Z") },
    new Bet(34, 10.78, 2.02, "client_5", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:08Z") },
    new Bet(34, 10.78, 2.02, "client_5", "event_9", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:10Z") },
    new Bet(35, 35.93, 1.75, "client_5", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:10Z") },
    new Bet(35, 35.93, 1.75, "client_5", "event_1", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:12Z") },
    new Bet(36, 33.22, 1.84, "client_1", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:12Z") },
    new Bet(36, 33.22, 1.84, "client_1", "event_5", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:15Z") },
    new Bet(37, 28.62, 2.38, "client_1", "event_8", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:14Z") },
    new Bet(37, 28.62, 2.38, "client_1", "event_8", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:17Z") },
    new Bet(38, 55.41, 2.11, "client_3", "event_4", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:16Z") },
    new Bet(38, 55.41, 2.11, "client_3", "event_4", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:19Z") },
    new Bet(39, 34.6, 2.63, "client_5", "event_7", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:18Z") },
    new Bet(39, 34.6, 2.63, "client_5", "event_7", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:19Z") },
    new Bet(40, 63.1, 2.22, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:20Z") },
    new Bet(40, 63.1, 2.22, "client_2", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:23Z") },
    new Bet(41, 13.1, 2.87, "client_3", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:22Z") },
    new Bet(41, 13.1, 2.87, "client_3", "event_5", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:23Z") },
    new Bet(42, 69.74, 2.76, "client_5", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:24Z") },
    new Bet(42, 69.74, 2.76, "client_5", "event_5", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:27Z") },
    new Bet(43, 39.19, 2.05, "client_1", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:26Z") },
    new Bet(43, 39.19, 2.05, "client_1", "event_1", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:29Z") },
    new Bet(44, 41.97, 2.3, "client_3", "event_8", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:28Z") },
    new Bet(44, 41.97, 2.3, "client_3", "event_8", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:29Z") },
    new Bet(45, 44.18, 1.8, "client_2", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:30Z") },
    new Bet(45, 44.18, 1.8, "client_2", "event_9", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:33Z") },
    new Bet(46, 54.3, 1.81, "client_1", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:32Z") },
    new Bet(46, 54.3, 1.81, "client_1", "event_9", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:33Z") },
    new Bet(47, 42.92, 1.61, "client_3", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:34Z") },
    new Bet(47, 42.92, 1.61, "client_3", "event_2", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:37Z") },
    new Bet(48, 82.07, 2.41, "client_2", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:36Z") },
    new Bet(48, 82.07, 2.41, "client_2", "event_1", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:37Z") },
    new Bet(49, 73.09, 2.95, "client_2", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:38Z") },
    new Bet(49, 73.09, 2.95, "client_2", "event_2", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:41Z") },
    new Bet(50, 97.54, 2.46, "client_5", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:40Z") },
    new Bet(50, 97.54, 2.46, "client_5", "event_1", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:43Z") },
    new Bet(51, 57.91, 2.72, "client_3", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:42Z") },
    new Bet(51, 57.91, 2.72, "client_3", "event_10", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:45Z") },
    new Bet(52, 38.51, 1.74, "client_2", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:44Z") },
    new Bet(52, 38.51, 1.74, "client_2", "event_5", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:47Z") },
    new Bet(53, 64.24, 1.65, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:46Z") },
    new Bet(53, 64.24, 1.65, "client_2", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:49Z") },
    new Bet(54, 91.15, 2.56, "client_1", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:48Z") },
    new Bet(54, 91.15, 2.56, "client_1", "event_1", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:51Z") },
    new Bet(55, 64.39, 2.72, "client_4", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:50Z") },
    new Bet(55, 64.39, 2.72, "client_4", "event_7", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:52Z") },
    new Bet(56, 86.49, 2.75, "client_2", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:52Z") },
    new Bet(56, 86.49, 2.75, "client_2", "event_10", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:54Z") },
    new Bet(57, 17.11, 2.86, "client_5", "event_7", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:54Z") },
    new Bet(57, 17.11, 2.86, "client_5", "event_7", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:55Z") },
    new Bet(58, 12.88, 1.69, "client_4", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:56Z") },
    new Bet(58, 12.88, 1.69, "client_4", "event_5", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:57Z") },
    new Bet(59, 22.12, 2.94, "client_5", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:58Z") },
    new Bet(59, 22.12, 2.94, "client_5", "event_9", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:01Z") },
    new Bet(60, 91.07, 2.73, "client_3", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:00Z") },
    new Bet(60, 91.07, 2.73, "client_3", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:03Z") },
    new Bet(61, 68.11, 2.55, "client_5", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:02Z") },
    new Bet(61, 68.11, 2.55, "client_5", "event_3", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:05Z") },
    new Bet(62, 56.74, 1.51, "client_2", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:04Z") },
    new Bet(62, 56.74, 1.51, "client_2", "event_3", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:07Z") },
    new Bet(63, 39.01, 2.7, "client_1", "event_6", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:06Z") },
    new Bet(63, 39.01, 2.7, "client_1", "event_6", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:08Z") },
    new Bet(64, 72.91, 2.15, "client_1", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:08Z") },
    new Bet(64, 72.91, 2.15, "client_1", "event_3", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:11Z") },
    new Bet(65, 71.93, 1.65, "client_4", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:10Z") },
    new Bet(65, 71.93, 1.65, "client_4", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:13Z") },
    new Bet(66, 66.76, 2.37, "client_2", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:12Z") },
    new Bet(66, 66.76, 2.37, "client_2", "event_5", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:14Z") },
    new Bet(67, 46.53, 1.56, "client_3", "event_8", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:14Z") },
    new Bet(67, 46.53, 1.56, "client_3", "event_8", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:16Z") },
    new Bet(68, 29.58, 1.81, "client_5", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:16Z") },
    new Bet(68, 29.58, 1.81, "client_5", "event_9", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:18Z") },
    new Bet(69, 12.52, 2.31, "client_4", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:18Z") },
    new Bet(69, 12.52, 2.31, "client_4", "event_10", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:19Z") },
    new Bet(70, 57.27, 1.84, "client_1", "event_6", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:20Z") },
    new Bet(70, 57.27, 1.84, "client_1", "event_6", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:21Z") },
    new Bet(71, 92.68, 1.79, "client_3", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:22Z") },
    new Bet(71, 92.68, 1.79, "client_3", "event_3", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:23Z") },
    new Bet(72, 75.42, 1.59, "client_2", "event_8", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:24Z") },
    new Bet(72, 75.42, 1.59, "client_2", "event_8", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:26Z") },
    new Bet(73, 39.24, 2.14, "client_3", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:26Z") },
    new Bet(73, 39.24, 2.14, "client_3", "event_2", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:27Z") },
    new Bet(74, 11.72, 2.79, "client_4", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:28Z") },
    new Bet(74, 11.72, 2.79, "client_4", "event_4", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:30Z") },
    new Bet(75, 69.54, 1.87, "client_1", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:30Z") },
    new Bet(75, 69.54, 1.87, "client_1", "event_4", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:32Z") },
    new Bet(76, 43.61, 2.89, "client_5", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:32Z") },
    new Bet(76, 43.61, 2.89, "client_5", "event_4", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:35Z") },
    new Bet(77, 75.96, 2.92, "client_2", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:34Z") },
    new Bet(77, 75.96, 2.92, "client_2", "event_2", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:35Z") },
    new Bet(78, 64.89, 2.15, "client_3", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:36Z") },
    new Bet(78, 64.89, 2.15, "client_3", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:38Z") },
    new Bet(79, 64.56, 1.72, "client_4", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:38Z") },
    new Bet(79, 64.56, 1.72, "client_4", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:41Z") },
    new Bet(80, 77.69, 2.52, "client_1", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:40Z") },
    new Bet(80, 77.69, 2.52, "client_1", "event_4", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:41Z") },
    new Bet(81, 77.09, 2.33, "client_5", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:42Z") },
    new Bet(81, 77.09, 2.33, "client_5", "event_6", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:45Z") },
    new Bet(82, 86.74, 2.3, "client_5", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:44Z") },
    new Bet(82, 86.74, 2.3, "client_5", "event_3", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:45Z") },
    new Bet(83, 82.36, 2.13, "client_5", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:46Z") },
    new Bet(83, 82.36, 2.13, "client_5", "event_9", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:49Z") },
    new Bet(84, 82.41, 2.9, "client_3", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:48Z") },
    new Bet(84, 82.41, 2.9, "client_3", "event_9", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:51Z") },
    new Bet(85, 81.07, 2.03, "client_1", "event_6", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:50Z") },
    new Bet(85, 81.07, 2.03, "client_1", "event_6", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:53Z") },
    new Bet(86, 26.27, 1.57, "client_3", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:52Z") },
    new Bet(86, 26.27, 1.57, "client_3", "event_9", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:54Z") },
    new Bet(87, 56.88, 1.95, "client_4", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:54Z") },
    new Bet(87, 56.88, 1.95, "client_4", "event_1", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:57Z") },
    new Bet(88, 17.62, 1.79, "client_1", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:56Z") },
    new Bet(88, 17.62, 1.79, "client_1", "event_10", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:59Z") },
    new Bet(89, 75.33, 1.89, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:58Z") },
    new Bet(89, 75.33, 1.89, "client_2", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:01Z") },
    new Bet(90, 68.23, 1.53, "client_5", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:00Z") },
    new Bet(90, 68.23, 1.53, "client_5", "event_2", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:03Z") },
    new Bet(91, 67.16, 2.57, "client_4", "event_8", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:02Z") },
    new Bet(91, 67.16, 2.57, "client_4", "event_8", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:03Z") },
    new Bet(92, 62.56, 2.62, "client_3", "event_10", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:04Z") },
    new Bet(92, 62.56, 2.62, "client_3", "event_10", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:06Z") },
    new Bet(93, 65.43, 2.27, "client_3", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:06Z") },
    new Bet(93, 65.43, 2.27, "client_3", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:08Z") },
    new Bet(94, 71.8, 1.95, "client_1", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:08Z") },
    new Bet(94, 71.8, 1.95, "client_1", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:10Z") },
    new Bet(95, 93.74, 2.52, "client_4", "event_4", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:10Z") },
    new Bet(95, 93.74, 2.52, "client_4", "event_4", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:12Z") },
    new Bet(96, 70.74, 2.29, "client_5", "event_6", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:12Z") },
    new Bet(96, 70.74, 2.29, "client_5", "event_6", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:13Z") },
    new Bet(97, 37.66, 2.68, "client_1", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:14Z") },
    new Bet(97, 37.66, 2.68, "client_1", "event_7", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:15Z") },
    new Bet(98, 12.89, 1.71, "client_3", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:16Z") },
    new Bet(98, 12.89, 1.71, "client_3", "event_9", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:19Z") },
    new Bet(99, 23.28, 1.82, "client_3", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:18Z") },
    new Bet(99, 23.28, 1.82, "client_3", "event_5", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:19Z") },
    new Bet(100, 50.89, 2.84, "client_1", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:20Z") },
    new Bet(100, 50.89, 2.84, "client_1", "event_7", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:22Z") } };

            var tasks = new List<Task>();

            stopwatch.Start();

            foreach (var testBet in testBets)
            {
                betHandlingService.Handle(testBet);
            }

            await betHandlingService.WhenAllHandled();

            Assert.Equal(100, savedBets.Count);

            stopwatch.Stop();
            var s = stopwatch.Elapsed;
        }

        [Fact]
        public async Task HandleAsync_HandleOneHundreadIncomingBetsThenShutdown_NoExceptions()
        {
            var mockRepository = new Mock<IBetRepository>();

            var savedBets = new List<BetCalculated>();

            mockRepository
                .Setup(repo => repo.Save(It.IsAny<IList<BetCalculated>>()))
                .Callback<IList<BetCalculated>>(b =>
                {
                    savedBets.AddRange(b);
                    Console.WriteLine("Saved bets");
                });

            Mock<IWorker> mockWorker = new Mock<IWorker>();

            mockWorker.Setup(x => x.CalculateBetAsync(It.IsAny<BetTransition>(), It.IsAny<CancellationToken>()))
                .Returns(
                    async (BetTransition bt, CancellationToken ct) =>
                    {
                        Random r = new Random();
                        await Task.Delay(r.Next(10, 100), ct);
                        ct.ThrowIfCancellationRequested();
                        return new BetCalculated(betTransition: null, betOutcome: BetOutcome.Won(250));
                    });

            Mock<IWorkersFactory> mockWokersFactory = new Mock<IWorkersFactory>();

            mockWokersFactory.Setup(x => x.CreateWorker())
                .Returns(mockWorker.Object);

            var betHandlingService = new BetHandlingService(
                new BetAgregator(),
                new WorkersDirector(maxWorkers: 10, mockWokersFactory.Object),
                new PersistancePolicy(),
                new DateTimeProvider(), // todo: mock date time provider
                mockRepository.Object);

            var testBets = new List<Bet>
{
    new Bet(1, 50.45, 2.57, "client_4", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:02Z") },
    new Bet(1, 50.45, 2.57, "client_4", "event_3", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:04Z") },
    new Bet(2, 81.16, 2.3, "client_2", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:04Z") },
    new Bet(2, 81.16, 2.3, "client_2", "event_2", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:05Z") },
    new Bet(3, 63.31, 1.72, "client_1", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:06Z") },
    new Bet(3, 63.31, 1.72, "client_1", "event_3", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:09Z") },
    new Bet(4, 34.2, 2.53, "client_2", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:08Z") },
    new Bet(4, 34.2, 2.53, "client_2", "event_2", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:09Z") },
    new Bet(5, 93.79, 2.35, "client_4", "event_10", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:10Z") },
    new Bet(5, 93.79, 2.35, "client_4", "event_10", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:12Z") },
    new Bet(6, 93.05, 1.91, "client_1", "event_8", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:12Z") },
    new Bet(6, 93.05, 1.91, "client_1", "event_8", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:13Z") },
    new Bet(7, 21.18, 1.6, "client_3", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:14Z") },
    new Bet(7, 21.18, 1.6, "client_3", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:17Z") },
    new Bet(8, 48.39, 2.16, "client_5", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:16Z") },
    new Bet(8, 48.39, 2.16, "client_5", "event_2", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:19Z") },
    new Bet(9, 80.83, 2.42, "client_4", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:18Z") },
    new Bet(9, 80.83, 2.42, "client_4", "event_1", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:19Z") },
    new Bet(10, 36.1, 2.29, "client_5", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:20Z") },
    new Bet(10, 36.1, 2.29, "client_5", "event_3", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:21Z") },
    new Bet(11, 56.04, 2.93, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:22Z") },
    new Bet(11, 56.04, 2.93, "client_2", "event_1", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:25Z") },
    new Bet(12, 19.46, 1.87, "client_2", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:24Z") },
    new Bet(12, 19.46, 1.87, "client_2", "event_5", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:25Z") },
    new Bet(13, 67.51, 2.2, "client_1", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:26Z") },
    new Bet(13, 67.51, 2.2, "client_1", "event_9", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:27Z") },
    new Bet(14, 13.12, 2.3, "client_2", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:28Z") },
    new Bet(14, 13.12, 2.3, "client_2", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:30Z") },
    new Bet(15, 35.57, 2.88, "client_3", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:30Z") },
    new Bet(15, 35.57, 2.88, "client_3", "event_5", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:32Z") },
    new Bet(16, 23.62, 1.87, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:32Z") },
    new Bet(16, 23.62, 1.87, "client_2", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:34Z") },
    new Bet(17, 62.65, 2.5, "client_2", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:34Z") },
    new Bet(17, 62.65, 2.5, "client_2", "event_2", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:37Z") },
    new Bet(18, 25.53, 1.71, "client_3", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:36Z") },
    new Bet(18, 25.53, 1.71, "client_3", "event_9", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:38Z") },
    new Bet(19, 82.0, 1.91, "client_3", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:38Z") },
    new Bet(19, 82.0, 1.91, "client_3", "event_7", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:40Z") },
    new Bet(20, 70.88, 1.83, "client_3", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:40Z") },
    new Bet(20, 70.88, 1.83, "client_3", "event_9", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:42Z") },
    new Bet(21, 47.22, 2.45, "client_1", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:42Z") },
    new Bet(21, 47.22, 2.45, "client_1", "event_10", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:43Z") },
    new Bet(22, 42.08, 2.34, "client_3", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:44Z") },
    new Bet(22, 42.08, 2.34, "client_3", "event_7", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:46Z") },
    new Bet(23, 37.14, 2.99, "client_4", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:46Z") },
    new Bet(23, 37.14, 2.99, "client_4", "event_9", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:47Z") },
    new Bet(24, 52.99, 1.63, "client_5", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:48Z") },
    new Bet(24, 52.99, 1.63, "client_5", "event_3", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:49Z") },
    new Bet(25, 45.15, 2.0, "client_4", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:50Z") },
    new Bet(25, 45.15, 2.0, "client_4", "event_5", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:53Z") },
    new Bet(26, 91.66, 2.62, "client_4", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:52Z") },
    new Bet(26, 91.66, 2.62, "client_4", "event_3", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:54Z") },
    new Bet(27, 24.79, 2.92, "client_1", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:54Z") },
    new Bet(27, 24.79, 2.92, "client_1", "event_1", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:55Z") },
    new Bet(28, 50.84, 2.61, "client_3", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:56Z") },
    new Bet(28, 50.84, 2.61, "client_3", "event_10", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:58Z") },
    new Bet(29, 73.43, 2.92, "client_4", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:58Z") },
    new Bet(29, 73.43, 2.92, "client_4", "event_4", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:00:59Z") },
    new Bet(30, 10.12, 1.76, "client_3", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:00Z") },
    new Bet(30, 10.12, 1.76, "client_3", "event_4", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:02Z") },
    new Bet(31, 53.82, 2.78, "client_4", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:02Z") },
    new Bet(31, 53.82, 2.78, "client_4", "event_2", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:03Z") },
    new Bet(32, 31.1, 1.8, "client_4", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:04Z") },
    new Bet(32, 31.1, 1.8, "client_4", "event_7", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:07Z") },
    new Bet(33, 30.94, 2.29, "client_1", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:06Z") },
    new Bet(33, 30.94, 2.29, "client_1", "event_4", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:08Z") },
    new Bet(34, 10.78, 2.02, "client_5", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:08Z") },
    new Bet(34, 10.78, 2.02, "client_5", "event_9", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:10Z") },
    new Bet(35, 35.93, 1.75, "client_5", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:10Z") },
    new Bet(35, 35.93, 1.75, "client_5", "event_1", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:12Z") },
    new Bet(36, 33.22, 1.84, "client_1", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:12Z") },
    new Bet(36, 33.22, 1.84, "client_1", "event_5", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:15Z") },
    new Bet(37, 28.62, 2.38, "client_1", "event_8", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:14Z") },
    new Bet(37, 28.62, 2.38, "client_1", "event_8", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:17Z") },
    new Bet(38, 55.41, 2.11, "client_3", "event_4", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:16Z") },
    new Bet(38, 55.41, 2.11, "client_3", "event_4", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:19Z") },
    new Bet(39, 34.6, 2.63, "client_5", "event_7", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:18Z") },
    new Bet(39, 34.6, 2.63, "client_5", "event_7", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:19Z") },
    new Bet(40, 63.1, 2.22, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:20Z") },
    new Bet(40, 63.1, 2.22, "client_2", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:23Z") },
    new Bet(41, 13.1, 2.87, "client_3", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:22Z") },
    new Bet(41, 13.1, 2.87, "client_3", "event_5", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:23Z") },
    new Bet(42, 69.74, 2.76, "client_5", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:24Z") },
    new Bet(42, 69.74, 2.76, "client_5", "event_5", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:27Z") },
    new Bet(43, 39.19, 2.05, "client_1", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:26Z") },
    new Bet(43, 39.19, 2.05, "client_1", "event_1", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:29Z") },
    new Bet(44, 41.97, 2.3, "client_3", "event_8", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:28Z") },
    new Bet(44, 41.97, 2.3, "client_3", "event_8", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:29Z") },
    new Bet(45, 44.18, 1.8, "client_2", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:30Z") },
    new Bet(45, 44.18, 1.8, "client_2", "event_9", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:33Z") },
    new Bet(46, 54.3, 1.81, "client_1", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:32Z") },
    new Bet(46, 54.3, 1.81, "client_1", "event_9", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:33Z") },
    new Bet(47, 42.92, 1.61, "client_3", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:34Z") },
    new Bet(47, 42.92, 1.61, "client_3", "event_2", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:37Z") },
    new Bet(48, 82.07, 2.41, "client_2", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:36Z") },
    new Bet(48, 82.07, 2.41, "client_2", "event_1", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:37Z") },
    new Bet(49, 73.09, 2.95, "client_2", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:38Z") },
    new Bet(49, 73.09, 2.95, "client_2", "event_2", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:41Z") },
    new Bet(50, 97.54, 2.46, "client_5", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:40Z") },
    new Bet(50, 97.54, 2.46, "client_5", "event_1", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:43Z") },
    new Bet(51, 57.91, 2.72, "client_3", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:42Z") },
    new Bet(51, 57.91, 2.72, "client_3", "event_10", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:45Z") },
    new Bet(52, 38.51, 1.74, "client_2", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:44Z") },
    new Bet(52, 38.51, 1.74, "client_2", "event_5", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:47Z") },
    new Bet(53, 64.24, 1.65, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:46Z") },
    new Bet(53, 64.24, 1.65, "client_2", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:49Z") },
    new Bet(54, 91.15, 2.56, "client_1", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:48Z") },
    new Bet(54, 91.15, 2.56, "client_1", "event_1", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:51Z") },
    new Bet(55, 64.39, 2.72, "client_4", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:50Z") },
    new Bet(55, 64.39, 2.72, "client_4", "event_7", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:52Z") },
    new Bet(56, 86.49, 2.75, "client_2", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:52Z") },
    new Bet(56, 86.49, 2.75, "client_2", "event_10", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:54Z") },
    new Bet(57, 17.11, 2.86, "client_5", "event_7", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:54Z") },
    new Bet(57, 17.11, 2.86, "client_5", "event_7", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:55Z") },
    new Bet(58, 12.88, 1.69, "client_4", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:56Z") },
    new Bet(58, 12.88, 1.69, "client_4", "event_5", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:57Z") },
    new Bet(59, 22.12, 2.94, "client_5", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:01:58Z") },
    new Bet(59, 22.12, 2.94, "client_5", "event_9", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:01Z") },
    new Bet(60, 91.07, 2.73, "client_3", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:00Z") },
    new Bet(60, 91.07, 2.73, "client_3", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:03Z") },
    new Bet(61, 68.11, 2.55, "client_5", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:02Z") },
    new Bet(61, 68.11, 2.55, "client_5", "event_3", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:05Z") },
    new Bet(62, 56.74, 1.51, "client_2", "event_3", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:04Z") },
    new Bet(62, 56.74, 1.51, "client_2", "event_3", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:07Z") },
    new Bet(63, 39.01, 2.7, "client_1", "event_6", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:06Z") },
    new Bet(63, 39.01, 2.7, "client_1", "event_6", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:08Z") },
    new Bet(64, 72.91, 2.15, "client_1", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:08Z") },
    new Bet(64, 72.91, 2.15, "client_1", "event_3", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:11Z") },
    new Bet(65, 71.93, 1.65, "client_4", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:10Z") },
    new Bet(65, 71.93, 1.65, "client_4", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:13Z") },
    new Bet(66, 66.76, 2.37, "client_2", "event_5", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:12Z") },
    new Bet(66, 66.76, 2.37, "client_2", "event_5", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:14Z") },
    new Bet(67, 46.53, 1.56, "client_3", "event_8", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:14Z") },
    new Bet(67, 46.53, 1.56, "client_3", "event_8", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:16Z") },
    new Bet(68, 29.58, 1.81, "client_5", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:16Z") },
    new Bet(68, 29.58, 1.81, "client_5", "event_9", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:18Z") },
    new Bet(69, 12.52, 2.31, "client_4", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:18Z") },
    new Bet(69, 12.52, 2.31, "client_4", "event_10", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:19Z") },
    new Bet(70, 57.27, 1.84, "client_1", "event_6", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:20Z") },
    new Bet(70, 57.27, 1.84, "client_1", "event_6", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:21Z") },
    new Bet(71, 92.68, 1.79, "client_3", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:22Z") },
    new Bet(71, 92.68, 1.79, "client_3", "event_3", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:23Z") },
    new Bet(72, 75.42, 1.59, "client_2", "event_8", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:24Z") },
    new Bet(72, 75.42, 1.59, "client_2", "event_8", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:26Z") },
    new Bet(73, 39.24, 2.14, "client_3", "event_2", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:26Z") },
    new Bet(73, 39.24, 2.14, "client_3", "event_2", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:27Z") },
    new Bet(74, 11.72, 2.79, "client_4", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:28Z") },
    new Bet(74, 11.72, 2.79, "client_4", "event_4", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:30Z") },
    new Bet(75, 69.54, 1.87, "client_1", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:30Z") },
    new Bet(75, 69.54, 1.87, "client_1", "event_4", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:32Z") },
    new Bet(76, 43.61, 2.89, "client_5", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:32Z") },
    new Bet(76, 43.61, 2.89, "client_5", "event_4", "winner", "TeamA", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:35Z") },
    new Bet(77, 75.96, 2.92, "client_2", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:34Z") },
    new Bet(77, 75.96, 2.92, "client_2", "event_2", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:35Z") },
    new Bet(78, 64.89, 2.15, "client_3", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:36Z") },
    new Bet(78, 64.89, 2.15, "client_3", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:38Z") },
    new Bet(79, 64.56, 1.72, "client_4", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:38Z") },
    new Bet(79, 64.56, 1.72, "client_4", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:41Z") },
    new Bet(80, 77.69, 2.52, "client_1", "event_4", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:40Z") },
    new Bet(80, 77.69, 2.52, "client_1", "event_4", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:41Z") },
    new Bet(81, 77.09, 2.33, "client_5", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:42Z") },
    new Bet(81, 77.09, 2.33, "client_5", "event_6", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:45Z") },
    new Bet(82, 86.74, 2.3, "client_5", "event_3", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:44Z") },
    new Bet(82, 86.74, 2.3, "client_5", "event_3", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:45Z") },
    new Bet(83, 82.36, 2.13, "client_5", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:46Z") },
    new Bet(83, 82.36, 2.13, "client_5", "event_9", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:49Z") },
    new Bet(84, 82.41, 2.9, "client_3", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:48Z") },
    new Bet(84, 82.41, 2.9, "client_3", "event_9", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:51Z") },
    new Bet(85, 81.07, 2.03, "client_1", "event_6", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:50Z") },
    new Bet(85, 81.07, 2.03, "client_1", "event_6", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:53Z") },
    new Bet(86, 26.27, 1.57, "client_3", "event_9", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:52Z") },
    new Bet(86, 26.27, 1.57, "client_3", "event_9", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:54Z") },
    new Bet(87, 56.88, 1.95, "client_4", "event_1", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:54Z") },
    new Bet(87, 56.88, 1.95, "client_4", "event_1", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:57Z") },
    new Bet(88, 17.62, 1.79, "client_1", "event_10", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:56Z") },
    new Bet(88, 17.62, 1.79, "client_1", "event_10", "winner", "TeamA", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:59Z") },
    new Bet(89, 75.33, 1.89, "client_2", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:02:58Z") },
    new Bet(89, 75.33, 1.89, "client_2", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:01Z") },
    new Bet(90, 68.23, 1.53, "client_5", "event_2", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:00Z") },
    new Bet(90, 68.23, 1.53, "client_5", "event_2", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:03Z") },
    new Bet(91, 67.16, 2.57, "client_4", "event_8", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:02Z") },
    new Bet(91, 67.16, 2.57, "client_4", "event_8", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:03Z") },
    new Bet(92, 62.56, 2.62, "client_3", "event_10", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:04Z") },
    new Bet(92, 62.56, 2.62, "client_3", "event_10", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:06Z") },
    new Bet(93, 65.43, 2.27, "client_3", "event_6", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:06Z") },
    new Bet(93, 65.43, 2.27, "client_3", "event_6", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:08Z") },
    new Bet(94, 71.8, 1.95, "client_1", "event_1", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:08Z") },
    new Bet(94, 71.8, 1.95, "client_1", "event_1", "winner", "TeamB", BetStatus.LOSER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:10Z") },
    new Bet(95, 93.74, 2.52, "client_4", "event_4", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:10Z") },
    new Bet(95, 93.74, 2.52, "client_4", "event_4", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:12Z") },
    new Bet(96, 70.74, 2.29, "client_5", "event_6", "winner", "TeamA", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:12Z") },
    new Bet(96, 70.74, 2.29, "client_5", "event_6", "winner", "TeamA", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:13Z") },
    new Bet(97, 37.66, 2.68, "client_1", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:14Z") },
    new Bet(97, 37.66, 2.68, "client_1", "event_7", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:15Z") },
    new Bet(98, 12.89, 1.71, "client_3", "event_9", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:16Z") },
    new Bet(98, 12.89, 1.71, "client_3", "event_9", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:19Z") },
    new Bet(99, 23.28, 1.82, "client_3", "event_5", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:18Z") },
    new Bet(99, 23.28, 1.82, "client_3", "event_5", "winner", "TeamB", BetStatus.VOID) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:19Z") },
    new Bet(100, 50.89, 2.84, "client_1", "event_7", "winner", "TeamB", BetStatus.OPEN) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:20Z") },
    new Bet(100, 50.89, 2.84, "client_1", "event_7", "winner", "TeamB", BetStatus.WINNER) { ArrivedUTC = DateTime.Parse("2025-06-04T10:03:22Z") } };

            var tasks = new List<Task>();

            foreach (var testBet in testBets)
            {
                betHandlingService.Handle(testBet);
            }

            await Task.Delay(500);

            betHandlingService.ShutDown();

            var surprise = savedBets.Count;
        }
    }
}
