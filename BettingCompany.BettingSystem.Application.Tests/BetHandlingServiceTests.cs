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
    new Bet(1, 293, 3.83, "Charlie", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(1, 293, 3.83, "Charlie", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.WINNER),
    new Bet(2, 397, 4.82, "Eve", "David vs Eve", "Handicap", "-1", BetStatus.OPEN),
    new Bet(2, 397, 4.82, "Eve", "David vs Eve", "Handicap", "-1", BetStatus.WINNER),
    new Bet(3, 309, 2.05, "Eve", "David vs Eve", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(3, 309, 2.05, "Eve", "David vs Eve", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(4, 129, 3.47, "David", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(4, 129, 3.47, "David", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(5, 206, 4.37, "Charlie", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(5, 206, 4.37, "Charlie", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(6, 288, 1.6, "David", "Charlie vs David", "Handicap", "-1", BetStatus.OPEN),
    new Bet(6, 288, 1.6, "David", "Charlie vs David", "Handicap", "-1", BetStatus.LOSER),
    new Bet(7, 457, 2.96, "Bob", "Charlie vs David", "Handicap", "+1", BetStatus.OPEN),
    new Bet(7, 457, 2.96, "Bob", "Charlie vs David", "Handicap", "+1", BetStatus.WINNER),
    new Bet(8, 178, 1.92, "Eve", "Bob vs Alice", "Handicap", "+1", BetStatus.OPEN),
    new Bet(8, 178, 1.92, "Eve", "Bob vs Alice", "Handicap", "+1", BetStatus.VOID),
    new Bet(9, 348, 4.74, "David", "Charlie vs David", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(9, 348, 4.74, "David", "Charlie vs David", "Correct Score", "2:2", BetStatus.VOID),
    new Bet(10, 383, 3.62, "Bob", "Bob vs Alice", "Handicap", "0", BetStatus.OPEN),
    new Bet(10, 383, 3.62, "Bob", "Bob vs Alice", "Handicap", "0", BetStatus.VOID),
    new Bet(11, 460, 4.29, "Eve", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(11, 460, 4.29, "Eve", "Bob vs Alice", "1X2", "2", BetStatus.WINNER),
    new Bet(12, 371, 2.2, "David", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(12, 371, 2.2, "David", "Charlie vs David", "Both Teams to Score", "No", BetStatus.WINNER),
    new Bet(13, 278, 4.76, "David", "David vs Eve", "1X2", "2", BetStatus.OPEN),
    new Bet(13, 278, 4.76, "David", "David vs Eve", "1X2", "2", BetStatus.VOID),
    new Bet(14, 76, 4.48, "Charlie", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(14, 76, 4.48, "Charlie", "Bob vs Alice", "1X2", "2", BetStatus.LOSER),
    new Bet(15, 217, 2.42, "Eve", "Charlie vs David", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(15, 217, 2.42, "Eve", "Charlie vs David", "Correct Score", "3:2", BetStatus.WINNER),
    new Bet(16, 210, 4.65, "David", "Bob vs Alice", "Handicap", "+1", BetStatus.OPEN),
    new Bet(16, 210, 4.65, "David", "Bob vs Alice", "Handicap", "+1", BetStatus.WINNER),
    new Bet(17, 40, 4.28, "Alice", "Bob vs Alice", "Correct Score", "2:1", BetStatus.OPEN),
    new Bet(17, 40, 4.28, "Alice", "Bob vs Alice", "Correct Score", "2:1", BetStatus.WINNER),
    new Bet(18, 398, 1.56, "David", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(18, 398, 1.56, "David", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.LOSER),
    new Bet(19, 293, 2.63, "Charlie", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(19, 293, 2.63, "Charlie", "Bob vs Alice", "1X2", "2", BetStatus.VOID),
    new Bet(20, 188, 3.37, "Alice", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(20, 188, 3.37, "Alice", "Bob vs Alice", "1X2", "2", BetStatus.WINNER),
    new Bet(21, 228, 2.58, "Charlie", "Bob vs Alice", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(21, 228, 2.58, "Charlie", "Bob vs Alice", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(22, 119, 2.28, "Alice", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(22, 119, 2.28, "Alice", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(23, 473, 2.86, "Charlie", "David vs Eve", "Correct Score", "0:0", BetStatus.OPEN),
    new Bet(23, 473, 2.86, "Charlie", "David vs Eve", "Correct Score", "0:0", BetStatus.WINNER),
    new Bet(24, 173, 2.88, "Alice", "Alice vs Charlie", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(24, 173, 2.88, "Alice", "Alice vs Charlie", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(25, 313, 2.29, "Bob", "Charlie vs David", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(25, 313, 2.29, "Bob", "Charlie vs David", "Correct Score", "3:2", BetStatus.VOID),
    new Bet(26, 124, 2.97, "Alice", "Alice vs Charlie", "1X2", "1", BetStatus.OPEN),
    new Bet(26, 124, 2.97, "Alice", "Alice vs Charlie", "1X2", "1", BetStatus.LOSER),
    new Bet(27, 372, 2.75, "Alice", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(27, 372, 2.75, "Alice", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.VOID),
    new Bet(28, 97, 4.67, "Charlie", "David vs Eve", "1X2", "2", BetStatus.OPEN),
    new Bet(28, 97, 4.67, "Charlie", "David vs Eve", "1X2", "2", BetStatus.LOSER),
    new Bet(29, 303, 1.74, "Charlie", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(29, 303, 1.74, "Charlie", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.LOSER),
    new Bet(30, 272, 4.26, "Bob", "Alice vs Charlie", "Handicap", "0", BetStatus.OPEN),
    new Bet(30, 272, 4.26, "Bob", "Alice vs Charlie", "Handicap", "0", BetStatus.LOSER),
    new Bet(31, 332, 1.51, "Charlie", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(31, 332, 1.51, "Charlie", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.LOSER),
    new Bet(32, 247, 3.79, "Bob", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(32, 247, 3.79, "Bob", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.WINNER),
    new Bet(33, 16, 4.09, "Bob", "Bob vs Alice", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(33, 16, 4.09, "Bob", "Bob vs Alice", "Correct Score", "3:2", BetStatus.VOID),
    new Bet(34, 287, 3.84, "Charlie", "Bob vs Alice", "1X2", "1", BetStatus.OPEN),
    new Bet(34, 287, 3.84, "Charlie", "Bob vs Alice", "1X2", "1", BetStatus.WINNER),
    new Bet(35, 355, 3.89, "Charlie", "Charlie vs David", "Handicap", "0", BetStatus.OPEN),
    new Bet(35, 355, 3.89, "Charlie", "Charlie vs David", "Handicap", "0", BetStatus.LOSER),
    new Bet(36, 165, 2.83, "Alice", "David vs Eve", "Correct Score", "2:1", BetStatus.OPEN),
    new Bet(36, 165, 2.83, "Alice", "David vs Eve", "Correct Score", "2:1", BetStatus.WINNER),
    new Bet(37, 439, 3.59, "Alice", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(37, 439, 3.59, "Alice", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(38, 481, 3.13, "David", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(38, 481, 3.13, "David", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.VOID),
    new Bet(39, 170, 2.19, "Bob", "Bob vs Alice", "Handicap", "0", BetStatus.OPEN),
    new Bet(39, 170, 2.19, "Bob", "Bob vs Alice", "Handicap", "0", BetStatus.LOSER),
    new Bet(40, 20, 1.86, "David", "David vs Eve", "1X2", "1", BetStatus.OPEN),
    new Bet(40, 20, 1.86, "David", "David vs Eve", "1X2", "1", BetStatus.WINNER),
    new Bet(41, 161, 4.4, "Bob", "Eve vs Bob", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(41, 161, 4.4, "Bob", "Eve vs Bob", "Correct Score", "2:2", BetStatus.LOSER),
    new Bet(42, 32, 2.48, "David", "Eve vs Bob", "1X2", "X", BetStatus.OPEN),
    new Bet(42, 32, 2.48, "David", "Eve vs Bob", "1X2", "X", BetStatus.LOSER),
    new Bet(43, 415, 4.95, "Bob", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(43, 415, 4.95, "Bob", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(44, 298, 2.32, "Alice", "Eve vs Bob", "Handicap", "-1", BetStatus.OPEN),
    new Bet(44, 298, 2.32, "Alice", "Eve vs Bob", "Handicap", "-1", BetStatus.LOSER),
    new Bet(45, 227, 2.63, "Eve", "David vs Eve", "Handicap", "-1", BetStatus.OPEN),
    new Bet(45, 227, 2.63, "Eve", "David vs Eve", "Handicap", "-1", BetStatus.LOSER),
    new Bet(46, 126, 2.15, "Charlie", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(46, 126, 2.15, "Charlie", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.WINNER),
    new Bet(47, 258, 4.69, "David", "Charlie vs David", "Handicap", "0", BetStatus.OPEN),
    new Bet(47, 258, 4.69, "David", "Charlie vs David", "Handicap", "0", BetStatus.LOSER),
    new Bet(48, 246, 3.18, "Alice", "Alice vs Charlie", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(48, 246, 3.18, "Alice", "Alice vs Charlie", "Total Goals", "Over 2.5", BetStatus.WINNER),
    new Bet(49, 297, 3.9, "Charlie", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(49, 297, 3.9, "Charlie", "Charlie vs David", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(50, 202, 2.21, "Charlie", "David vs Eve", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(50, 202, 2.21, "Charlie", "David vs Eve", "Total Goals", "Over 2.5", BetStatus.WINNER),
    new Bet(51, 430, 2.36, "Eve", "Charlie vs David", "Correct Score", "0:0", BetStatus.OPEN),
    new Bet(51, 430, 2.36, "Eve", "Charlie vs David", "Correct Score", "0:0", BetStatus.LOSER),
    new Bet(52, 89, 1.72, "Bob", "Alice vs Charlie", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(52, 89, 1.72, "Bob", "Alice vs Charlie", "Both Teams to Score", "Yes", BetStatus.WINNER),
    new Bet(53, 382, 5.0, "Charlie", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(53, 382, 5.0, "Charlie", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(54, 466, 3.0, "Bob", "Eve vs Bob", "1X2", "2", BetStatus.OPEN),
    new Bet(54, 466, 3.0, "Bob", "Eve vs Bob", "1X2", "2", BetStatus.VOID),
    new Bet(55, 319, 2.24, "Bob", "Bob vs Alice", "1X2", "1", BetStatus.OPEN),
    new Bet(55, 319, 2.24, "Bob", "Bob vs Alice", "1X2", "1", BetStatus.LOSER),
    new Bet(56, 337, 3.13, "Eve", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(56, 337, 3.13, "Eve", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(57, 99, 4.07, "Charlie", "Alice vs Charlie", "1X2", "X", BetStatus.OPEN),
    new Bet(57, 99, 4.07, "Charlie", "Alice vs Charlie", "1X2", "X", BetStatus.LOSER),
    new Bet(58, 427, 3.16, "Alice", "Eve vs Bob", "1X2", "X", BetStatus.OPEN),
    new Bet(58, 427, 3.16, "Alice", "Eve vs Bob", "1X2", "X", BetStatus.LOSER),
    new Bet(59, 263, 4.12, "David", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(59, 263, 4.12, "David", "Bob vs Alice", "1X2", "2", BetStatus.WINNER),
    new Bet(60, 267, 2.72, "David", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(60, 267, 2.72, "David", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.WINNER),
    new Bet(61, 433, 4.98, "Eve", "Eve vs Bob", "Handicap", "0", BetStatus.OPEN),
    new Bet(61, 433, 4.98, "Eve", "Eve vs Bob", "Handicap", "0", BetStatus.WINNER),
    new Bet(62, 81, 3.8, "Alice", "Bob vs Alice", "1X2", "X", BetStatus.OPEN),
    new Bet(62, 81, 3.8, "Alice", "Bob vs Alice", "1X2", "X", BetStatus.WINNER),
    new Bet(63, 35, 4.99, "David", "Charlie vs David", "Correct Score", "1:0", BetStatus.OPEN),
    new Bet(63, 35, 4.99, "David", "Charlie vs David", "Correct Score", "1:0", BetStatus.LOSER),
    new Bet(64, 91, 3.34, "Alice", "Charlie vs David", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(64, 91, 3.34, "Alice", "Charlie vs David", "Correct Score", "3:2", BetStatus.LOSER),
    new Bet(65, 480, 2.1, "Eve", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(65, 480, 2.1, "Eve", "Charlie vs David", "Both Teams to Score", "No", BetStatus.WINNER),
    new Bet(66, 45, 3.11, "Alice", "David vs Eve", "1X2", "2", BetStatus.OPEN),
    new Bet(66, 45, 3.11, "Alice", "David vs Eve", "1X2", "2", BetStatus.LOSER),
    new Bet(67, 312, 1.73, "Charlie", "Charlie vs David", "1X2", "1", BetStatus.OPEN),
    new Bet(67, 312, 1.73, "Charlie", "Charlie vs David", "1X2", "1", BetStatus.WINNER),
    new Bet(68, 19, 1.75, "David", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(68, 19, 1.75, "David", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.LOSER),
    new Bet(69, 348, 1.83, "Bob", "Bob vs Alice", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(69, 348, 1.83, "Bob", "Bob vs Alice", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(70, 225, 1.99, "David", "Alice vs Charlie", "1X2", "X", BetStatus.OPEN),
    new Bet(70, 225, 1.99, "David", "Alice vs Charlie", "1X2", "X", BetStatus.VOID),
    new Bet(71, 290, 3.68, "Eve", "Alice vs Charlie", "1X2", "2", BetStatus.OPEN),
    new Bet(71, 290, 3.68, "Eve", "Alice vs Charlie", "1X2", "2", BetStatus.WINNER),
    new Bet(72, 52, 3.25, "Alice", "Bob vs Alice", "Correct Score", "0:0", BetStatus.OPEN),
    new Bet(72, 52, 3.25, "Alice", "Bob vs Alice", "Correct Score", "0:0", BetStatus.LOSER),
    new Bet(73, 493, 3.15, "Alice", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(73, 493, 3.15, "Alice", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.LOSER),
    new Bet(74, 172, 4.87, "Charlie", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(74, 172, 4.87, "Charlie", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(75, 208, 3.81, "Alice", "Bob vs Alice", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(75, 208, 3.81, "Alice", "Bob vs Alice", "Both Teams to Score", "Yes", BetStatus.VOID),
    new Bet(76, 465, 2.25, "Eve", "Charlie vs David", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(76, 465, 2.25, "Eve", "Charlie vs David", "Total Goals", "Over 2.5", BetStatus.LOSER),
    new Bet(77, 59, 3.68, "Charlie", "Eve vs Bob", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(77, 59, 3.68, "Charlie", "Eve vs Bob", "Correct Score", "3:2", BetStatus.VOID),
    new Bet(78, 438, 4.13, "David", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(78, 438, 4.13, "David", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(79, 78, 3.66, "Eve", "Charlie vs David", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(79, 78, 3.66, "Eve", "Charlie vs David", "Total Goals", "Over 2.5", BetStatus.WINNER),
    new Bet(80, 176, 2.1, "Bob", "Alice vs Charlie", "Handicap", "0", BetStatus.OPEN),
    new Bet(80, 176, 2.1, "Bob", "Alice vs Charlie", "Handicap", "0", BetStatus.WINNER),
    new Bet(81, 208, 2.04, "Alice", "Charlie vs David", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(81, 208, 2.04, "Alice", "Charlie vs David", "Correct Score", "3:2", BetStatus.VOID),
    new Bet(82, 286, 2.92, "Bob", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(82, 286, 2.92, "Bob", "Charlie vs David", "Both Teams to Score", "No", BetStatus.LOSER),
    new Bet(83, 417, 1.77, "David", "Charlie vs David", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(83, 417, 1.77, "David", "Charlie vs David", "Correct Score", "2:2", BetStatus.VOID),
    new Bet(84, 193, 3.85, "Alice", "Bob vs Alice", "1X2", "1", BetStatus.OPEN),
    new Bet(84, 193, 3.85, "Alice", "Bob vs Alice", "1X2", "1", BetStatus.VOID),
    new Bet(85, 21, 4.96, "Charlie", "Charlie vs David", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(85, 21, 4.96, "Charlie", "Charlie vs David", "Both Teams to Score", "Yes", BetStatus.LOSER),
    new Bet(86, 67, 4.64, "Bob", "Bob vs Alice", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(86, 67, 4.64, "Bob", "Bob vs Alice", "Correct Score", "3:2", BetStatus.WINNER),
    new Bet(87, 75, 4.16, "Eve", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(87, 75, 4.16, "Eve", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.WINNER),
    new Bet(88, 225, 2.93, "Alice", "Bob vs Alice", "Correct Score", "1:0", BetStatus.OPEN),
    new Bet(88, 225, 2.93, "Alice", "Bob vs Alice", "Correct Score", "1:0", BetStatus.VOID),
    new Bet(89, 414, 3.7, "David", "David vs Eve", "Handicap", "-1", BetStatus.OPEN),
    new Bet(89, 414, 3.7, "David", "David vs Eve", "Handicap", "-1", BetStatus.VOID),
    new Bet(90, 341, 2.35, "Alice", "Eve vs Bob", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(90, 341, 2.35, "Alice", "Eve vs Bob", "Correct Score", "2:2", BetStatus.VOID),
    new Bet(91, 180, 4.6, "David", "Bob vs Alice", "Correct Score", "0:0", BetStatus.OPEN),
    new Bet(91, 180, 4.6, "David", "Bob vs Alice", "Correct Score", "0:0", BetStatus.LOSER),
    new Bet(92, 467, 1.56, "Bob", "Alice vs Charlie", "Handicap", "+1", BetStatus.OPEN),
    new Bet(92, 467, 1.56, "Bob", "Alice vs Charlie", "Handicap", "+1", BetStatus.VOID),
    new Bet(93, 353, 4.47, "David", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(93, 353, 4.47, "David", "Charlie vs David", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(94, 494, 4.83, "Charlie", "Alice vs Charlie", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(94, 494, 4.83, "Charlie", "Alice vs Charlie", "Correct Score", "2:2", BetStatus.VOID),
    new Bet(95, 189, 3.33, "Bob", "Eve vs Bob", "Correct Score", "2:1", BetStatus.OPEN),
    new Bet(95, 189, 3.33, "Bob", "Eve vs Bob", "Correct Score", "2:1", BetStatus.VOID),
    new Bet(96, 440, 4.29, "Alice", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(96, 440, 4.29, "Alice", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.LOSER),
    new Bet(97, 455, 4.77, "Charlie", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(97, 455, 4.77, "Charlie", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(98, 269, 2.3, "Alice", "David vs Eve", "1X2", "X", BetStatus.OPEN),
    new Bet(98, 269, 2.3, "Alice", "David vs Eve", "1X2", "X", BetStatus.WINNER),
    new Bet(99, 67, 3.49, "Alice", "Charlie vs David", "Handicap", "+1", BetStatus.OPEN),
    new Bet(99, 67, 3.49, "Alice", "Charlie vs David", "Handicap", "+1", BetStatus.LOSER),
    new Bet(100, 109, 2.06, "Charlie", "Eve vs Bob", "Handicap", "0", BetStatus.OPEN),
    new Bet(100, 109, 2.06, "Charlie", "Eve vs Bob", "Handicap", "0", BetStatus.VOID),
};

            var tasks = new List<Task>();

            foreach (var testBet in testBets)
            {
                betHandlingService.Handle(testBet);
            }

            await betHandlingService.WhenAllHandled();

            Assert.Equal(100, savedBets.Count);

            Assert.All(savedBets, (bet) => { Assert.NotNull(bet.BetOutcome); });
        }

        [Fact]
        public async Task HandleAsync_HandleOneHundreadIncomingBetsThenShutdown_NoExceptions()
        {
            var mockRepository = new Mock<IBetRepository>();

            var savedBets = new List<BetCalculated>();

            mockRepository
                .Setup(repo => repo.Save(It.IsAny<IList<BetCalculated>>()))
                .Callback<IList<BetCalculated>>(bets =>
                {
                    savedBets.AddRange(bets);
                });

            Mock<IWorker> mockWorker = new();

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
    new Bet(1, 293, 3.83, "Charlie", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(1, 293, 3.83, "Charlie", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.WINNER),
    new Bet(2, 397, 4.82, "Eve", "David vs Eve", "Handicap", "-1", BetStatus.OPEN),
    new Bet(2, 397, 4.82, "Eve", "David vs Eve", "Handicap", "-1", BetStatus.WINNER),
    new Bet(3, 309, 2.05, "Eve", "David vs Eve", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(3, 309, 2.05, "Eve", "David vs Eve", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(4, 129, 3.47, "David", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(4, 129, 3.47, "David", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(5, 206, 4.37, "Charlie", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(5, 206, 4.37, "Charlie", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(6, 288, 1.6, "David", "Charlie vs David", "Handicap", "-1", BetStatus.OPEN),
    new Bet(6, 288, 1.6, "David", "Charlie vs David", "Handicap", "-1", BetStatus.LOSER),
    new Bet(7, 457, 2.96, "Bob", "Charlie vs David", "Handicap", "+1", BetStatus.OPEN),
    new Bet(7, 457, 2.96, "Bob", "Charlie vs David", "Handicap", "+1", BetStatus.WINNER),
    new Bet(8, 178, 1.92, "Eve", "Bob vs Alice", "Handicap", "+1", BetStatus.OPEN),
    new Bet(8, 178, 1.92, "Eve", "Bob vs Alice", "Handicap", "+1", BetStatus.VOID),
    new Bet(9, 348, 4.74, "David", "Charlie vs David", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(9, 348, 4.74, "David", "Charlie vs David", "Correct Score", "2:2", BetStatus.VOID),
    new Bet(10, 383, 3.62, "Bob", "Bob vs Alice", "Handicap", "0", BetStatus.OPEN),
    new Bet(10, 383, 3.62, "Bob", "Bob vs Alice", "Handicap", "0", BetStatus.VOID),
    new Bet(11, 460, 4.29, "Eve", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(11, 460, 4.29, "Eve", "Bob vs Alice", "1X2", "2", BetStatus.WINNER),
    new Bet(12, 371, 2.2, "David", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(12, 371, 2.2, "David", "Charlie vs David", "Both Teams to Score", "No", BetStatus.WINNER),
    new Bet(13, 278, 4.76, "David", "David vs Eve", "1X2", "2", BetStatus.OPEN),
    new Bet(13, 278, 4.76, "David", "David vs Eve", "1X2", "2", BetStatus.VOID),
    new Bet(14, 76, 4.48, "Charlie", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(14, 76, 4.48, "Charlie", "Bob vs Alice", "1X2", "2", BetStatus.LOSER),
    new Bet(15, 217, 2.42, "Eve", "Charlie vs David", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(15, 217, 2.42, "Eve", "Charlie vs David", "Correct Score", "3:2", BetStatus.WINNER),
    new Bet(16, 210, 4.65, "David", "Bob vs Alice", "Handicap", "+1", BetStatus.OPEN),
    new Bet(16, 210, 4.65, "David", "Bob vs Alice", "Handicap", "+1", BetStatus.WINNER),
    new Bet(17, 40, 4.28, "Alice", "Bob vs Alice", "Correct Score", "2:1", BetStatus.OPEN),
    new Bet(17, 40, 4.28, "Alice", "Bob vs Alice", "Correct Score", "2:1", BetStatus.WINNER),
    new Bet(18, 398, 1.56, "David", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(18, 398, 1.56, "David", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.LOSER),
    new Bet(19, 293, 2.63, "Charlie", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(19, 293, 2.63, "Charlie", "Bob vs Alice", "1X2", "2", BetStatus.VOID),
    new Bet(20, 188, 3.37, "Alice", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(20, 188, 3.37, "Alice", "Bob vs Alice", "1X2", "2", BetStatus.WINNER),
    new Bet(21, 228, 2.58, "Charlie", "Bob vs Alice", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(21, 228, 2.58, "Charlie", "Bob vs Alice", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(22, 119, 2.28, "Alice", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(22, 119, 2.28, "Alice", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(23, 473, 2.86, "Charlie", "David vs Eve", "Correct Score", "0:0", BetStatus.OPEN),
    new Bet(23, 473, 2.86, "Charlie", "David vs Eve", "Correct Score", "0:0", BetStatus.WINNER),
    new Bet(24, 173, 2.88, "Alice", "Alice vs Charlie", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(24, 173, 2.88, "Alice", "Alice vs Charlie", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(25, 313, 2.29, "Bob", "Charlie vs David", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(25, 313, 2.29, "Bob", "Charlie vs David", "Correct Score", "3:2", BetStatus.VOID),
    new Bet(26, 124, 2.97, "Alice", "Alice vs Charlie", "1X2", "1", BetStatus.OPEN),
    new Bet(26, 124, 2.97, "Alice", "Alice vs Charlie", "1X2", "1", BetStatus.LOSER),
    new Bet(27, 372, 2.75, "Alice", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(27, 372, 2.75, "Alice", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.VOID),
    new Bet(28, 97, 4.67, "Charlie", "David vs Eve", "1X2", "2", BetStatus.OPEN),
    new Bet(28, 97, 4.67, "Charlie", "David vs Eve", "1X2", "2", BetStatus.LOSER),
    new Bet(29, 303, 1.74, "Charlie", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(29, 303, 1.74, "Charlie", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.LOSER),
    new Bet(30, 272, 4.26, "Bob", "Alice vs Charlie", "Handicap", "0", BetStatus.OPEN),
    new Bet(30, 272, 4.26, "Bob", "Alice vs Charlie", "Handicap", "0", BetStatus.LOSER),
    new Bet(31, 332, 1.51, "Charlie", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(31, 332, 1.51, "Charlie", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.LOSER),
    new Bet(32, 247, 3.79, "Bob", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(32, 247, 3.79, "Bob", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.WINNER),
    new Bet(33, 16, 4.09, "Bob", "Bob vs Alice", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(33, 16, 4.09, "Bob", "Bob vs Alice", "Correct Score", "3:2", BetStatus.VOID),
    new Bet(34, 287, 3.84, "Charlie", "Bob vs Alice", "1X2", "1", BetStatus.OPEN),
    new Bet(34, 287, 3.84, "Charlie", "Bob vs Alice", "1X2", "1", BetStatus.WINNER),
    new Bet(35, 355, 3.89, "Charlie", "Charlie vs David", "Handicap", "0", BetStatus.OPEN),
    new Bet(35, 355, 3.89, "Charlie", "Charlie vs David", "Handicap", "0", BetStatus.LOSER),
    new Bet(36, 165, 2.83, "Alice", "David vs Eve", "Correct Score", "2:1", BetStatus.OPEN),
    new Bet(36, 165, 2.83, "Alice", "David vs Eve", "Correct Score", "2:1", BetStatus.WINNER),
    new Bet(37, 439, 3.59, "Alice", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(37, 439, 3.59, "Alice", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(38, 481, 3.13, "David", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(38, 481, 3.13, "David", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.VOID),
    new Bet(39, 170, 2.19, "Bob", "Bob vs Alice", "Handicap", "0", BetStatus.OPEN),
    new Bet(39, 170, 2.19, "Bob", "Bob vs Alice", "Handicap", "0", BetStatus.LOSER),
    new Bet(40, 20, 1.86, "David", "David vs Eve", "1X2", "1", BetStatus.OPEN),
    new Bet(40, 20, 1.86, "David", "David vs Eve", "1X2", "1", BetStatus.WINNER),
    new Bet(41, 161, 4.4, "Bob", "Eve vs Bob", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(41, 161, 4.4, "Bob", "Eve vs Bob", "Correct Score", "2:2", BetStatus.LOSER),
    new Bet(42, 32, 2.48, "David", "Eve vs Bob", "1X2", "X", BetStatus.OPEN),
    new Bet(42, 32, 2.48, "David", "Eve vs Bob", "1X2", "X", BetStatus.LOSER),
    new Bet(43, 415, 4.95, "Bob", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(43, 415, 4.95, "Bob", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(44, 298, 2.32, "Alice", "Eve vs Bob", "Handicap", "-1", BetStatus.OPEN),
    new Bet(44, 298, 2.32, "Alice", "Eve vs Bob", "Handicap", "-1", BetStatus.LOSER),
    new Bet(45, 227, 2.63, "Eve", "David vs Eve", "Handicap", "-1", BetStatus.OPEN),
    new Bet(45, 227, 2.63, "Eve", "David vs Eve", "Handicap", "-1", BetStatus.LOSER),
    new Bet(46, 126, 2.15, "Charlie", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(46, 126, 2.15, "Charlie", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.WINNER),
    new Bet(47, 258, 4.69, "David", "Charlie vs David", "Handicap", "0", BetStatus.OPEN),
    new Bet(47, 258, 4.69, "David", "Charlie vs David", "Handicap", "0", BetStatus.LOSER),
    new Bet(48, 246, 3.18, "Alice", "Alice vs Charlie", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(48, 246, 3.18, "Alice", "Alice vs Charlie", "Total Goals", "Over 2.5", BetStatus.WINNER),
    new Bet(49, 297, 3.9, "Charlie", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(49, 297, 3.9, "Charlie", "Charlie vs David", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(50, 202, 2.21, "Charlie", "David vs Eve", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(50, 202, 2.21, "Charlie", "David vs Eve", "Total Goals", "Over 2.5", BetStatus.WINNER),
    new Bet(51, 430, 2.36, "Eve", "Charlie vs David", "Correct Score", "0:0", BetStatus.OPEN),
    new Bet(51, 430, 2.36, "Eve", "Charlie vs David", "Correct Score", "0:0", BetStatus.LOSER),
    new Bet(52, 89, 1.72, "Bob", "Alice vs Charlie", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(52, 89, 1.72, "Bob", "Alice vs Charlie", "Both Teams to Score", "Yes", BetStatus.WINNER),
    new Bet(53, 382, 5.0, "Charlie", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(53, 382, 5.0, "Charlie", "Eve vs Bob", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(54, 466, 3.0, "Bob", "Eve vs Bob", "1X2", "2", BetStatus.OPEN),
    new Bet(54, 466, 3.0, "Bob", "Eve vs Bob", "1X2", "2", BetStatus.VOID),
    new Bet(55, 319, 2.24, "Bob", "Bob vs Alice", "1X2", "1", BetStatus.OPEN),
    new Bet(55, 319, 2.24, "Bob", "Bob vs Alice", "1X2", "1", BetStatus.LOSER),
    new Bet(56, 337, 3.13, "Eve", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(56, 337, 3.13, "Eve", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(57, 99, 4.07, "Charlie", "Alice vs Charlie", "1X2", "X", BetStatus.OPEN),
    new Bet(57, 99, 4.07, "Charlie", "Alice vs Charlie", "1X2", "X", BetStatus.LOSER),
    new Bet(58, 427, 3.16, "Alice", "Eve vs Bob", "1X2", "X", BetStatus.OPEN),
    new Bet(58, 427, 3.16, "Alice", "Eve vs Bob", "1X2", "X", BetStatus.LOSER),
    new Bet(59, 263, 4.12, "David", "Bob vs Alice", "1X2", "2", BetStatus.OPEN),
    new Bet(59, 263, 4.12, "David", "Bob vs Alice", "1X2", "2", BetStatus.WINNER),
    new Bet(60, 267, 2.72, "David", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(60, 267, 2.72, "David", "Eve vs Bob", "Both Teams to Score", "Yes", BetStatus.WINNER),
    new Bet(61, 433, 4.98, "Eve", "Eve vs Bob", "Handicap", "0", BetStatus.OPEN),
    new Bet(61, 433, 4.98, "Eve", "Eve vs Bob", "Handicap", "0", BetStatus.WINNER),
    new Bet(62, 81, 3.8, "Alice", "Bob vs Alice", "1X2", "X", BetStatus.OPEN),
    new Bet(62, 81, 3.8, "Alice", "Bob vs Alice", "1X2", "X", BetStatus.WINNER),
    new Bet(63, 35, 4.99, "David", "Charlie vs David", "Correct Score", "1:0", BetStatus.OPEN),
    new Bet(63, 35, 4.99, "David", "Charlie vs David", "Correct Score", "1:0", BetStatus.LOSER),
    new Bet(64, 91, 3.34, "Alice", "Charlie vs David", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(64, 91, 3.34, "Alice", "Charlie vs David", "Correct Score", "3:2", BetStatus.LOSER),
    new Bet(65, 480, 2.1, "Eve", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(65, 480, 2.1, "Eve", "Charlie vs David", "Both Teams to Score", "No", BetStatus.WINNER),
    new Bet(66, 45, 3.11, "Alice", "David vs Eve", "1X2", "2", BetStatus.OPEN),
    new Bet(66, 45, 3.11, "Alice", "David vs Eve", "1X2", "2", BetStatus.LOSER),
    new Bet(67, 312, 1.73, "Charlie", "Charlie vs David", "1X2", "1", BetStatus.OPEN),
    new Bet(67, 312, 1.73, "Charlie", "Charlie vs David", "1X2", "1", BetStatus.WINNER),
    new Bet(68, 19, 1.75, "David", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(68, 19, 1.75, "David", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.LOSER),
    new Bet(69, 348, 1.83, "Bob", "Bob vs Alice", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(69, 348, 1.83, "Bob", "Bob vs Alice", "Total Goals", "Under 2.5", BetStatus.VOID),
    new Bet(70, 225, 1.99, "David", "Alice vs Charlie", "1X2", "X", BetStatus.OPEN),
    new Bet(70, 225, 1.99, "David", "Alice vs Charlie", "1X2", "X", BetStatus.VOID),
    new Bet(71, 290, 3.68, "Eve", "Alice vs Charlie", "1X2", "2", BetStatus.OPEN),
    new Bet(71, 290, 3.68, "Eve", "Alice vs Charlie", "1X2", "2", BetStatus.WINNER),
    new Bet(72, 52, 3.25, "Alice", "Bob vs Alice", "Correct Score", "0:0", BetStatus.OPEN),
    new Bet(72, 52, 3.25, "Alice", "Bob vs Alice", "Correct Score", "0:0", BetStatus.LOSER),
    new Bet(73, 493, 3.15, "Alice", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.OPEN),
    new Bet(73, 493, 3.15, "Alice", "Eve vs Bob", "Total Goals", "Under 2.5", BetStatus.LOSER),
    new Bet(74, 172, 4.87, "Charlie", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(74, 172, 4.87, "Charlie", "Alice vs Charlie", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(75, 208, 3.81, "Alice", "Bob vs Alice", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(75, 208, 3.81, "Alice", "Bob vs Alice", "Both Teams to Score", "Yes", BetStatus.VOID),
    new Bet(76, 465, 2.25, "Eve", "Charlie vs David", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(76, 465, 2.25, "Eve", "Charlie vs David", "Total Goals", "Over 2.5", BetStatus.LOSER),
    new Bet(77, 59, 3.68, "Charlie", "Eve vs Bob", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(77, 59, 3.68, "Charlie", "Eve vs Bob", "Correct Score", "3:2", BetStatus.VOID),
    new Bet(78, 438, 4.13, "David", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(78, 438, 4.13, "David", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(79, 78, 3.66, "Eve", "Charlie vs David", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(79, 78, 3.66, "Eve", "Charlie vs David", "Total Goals", "Over 2.5", BetStatus.WINNER),
    new Bet(80, 176, 2.1, "Bob", "Alice vs Charlie", "Handicap", "0", BetStatus.OPEN),
    new Bet(80, 176, 2.1, "Bob", "Alice vs Charlie", "Handicap", "0", BetStatus.WINNER),
    new Bet(81, 208, 2.04, "Alice", "Charlie vs David", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(81, 208, 2.04, "Alice", "Charlie vs David", "Correct Score", "3:2", BetStatus.VOID),
    new Bet(82, 286, 2.92, "Bob", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(82, 286, 2.92, "Bob", "Charlie vs David", "Both Teams to Score", "No", BetStatus.LOSER),
    new Bet(83, 417, 1.77, "David", "Charlie vs David", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(83, 417, 1.77, "David", "Charlie vs David", "Correct Score", "2:2", BetStatus.VOID),
    new Bet(84, 193, 3.85, "Alice", "Bob vs Alice", "1X2", "1", BetStatus.OPEN),
    new Bet(84, 193, 3.85, "Alice", "Bob vs Alice", "1X2", "1", BetStatus.VOID),
    new Bet(85, 21, 4.96, "Charlie", "Charlie vs David", "Both Teams to Score", "Yes", BetStatus.OPEN),
    new Bet(85, 21, 4.96, "Charlie", "Charlie vs David", "Both Teams to Score", "Yes", BetStatus.LOSER),
    new Bet(86, 67, 4.64, "Bob", "Bob vs Alice", "Correct Score", "3:2", BetStatus.OPEN),
    new Bet(86, 67, 4.64, "Bob", "Bob vs Alice", "Correct Score", "3:2", BetStatus.WINNER),
    new Bet(87, 75, 4.16, "Eve", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(87, 75, 4.16, "Eve", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.WINNER),
    new Bet(88, 225, 2.93, "Alice", "Bob vs Alice", "Correct Score", "1:0", BetStatus.OPEN),
    new Bet(88, 225, 2.93, "Alice", "Bob vs Alice", "Correct Score", "1:0", BetStatus.VOID),
    new Bet(89, 414, 3.7, "David", "David vs Eve", "Handicap", "-1", BetStatus.OPEN),
    new Bet(89, 414, 3.7, "David", "David vs Eve", "Handicap", "-1", BetStatus.VOID),
    new Bet(90, 341, 2.35, "Alice", "Eve vs Bob", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(90, 341, 2.35, "Alice", "Eve vs Bob", "Correct Score", "2:2", BetStatus.VOID),
    new Bet(91, 180, 4.6, "David", "Bob vs Alice", "Correct Score", "0:0", BetStatus.OPEN),
    new Bet(91, 180, 4.6, "David", "Bob vs Alice", "Correct Score", "0:0", BetStatus.LOSER),
    new Bet(92, 467, 1.56, "Bob", "Alice vs Charlie", "Handicap", "+1", BetStatus.OPEN),
    new Bet(92, 467, 1.56, "Bob", "Alice vs Charlie", "Handicap", "+1", BetStatus.VOID),
    new Bet(93, 353, 4.47, "David", "Charlie vs David", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(93, 353, 4.47, "David", "Charlie vs David", "Both Teams to Score", "No", BetStatus.VOID),
    new Bet(94, 494, 4.83, "Charlie", "Alice vs Charlie", "Correct Score", "2:2", BetStatus.OPEN),
    new Bet(94, 494, 4.83, "Charlie", "Alice vs Charlie", "Correct Score", "2:2", BetStatus.VOID),
    new Bet(95, 189, 3.33, "Bob", "Eve vs Bob", "Correct Score", "2:1", BetStatus.OPEN),
    new Bet(95, 189, 3.33, "Bob", "Eve vs Bob", "Correct Score", "2:1", BetStatus.VOID),
    new Bet(96, 440, 4.29, "Alice", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.OPEN),
    new Bet(96, 440, 4.29, "Alice", "Eve vs Bob", "Both Teams to Score", "No", BetStatus.LOSER),
    new Bet(97, 455, 4.77, "Charlie", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.OPEN),
    new Bet(97, 455, 4.77, "Charlie", "Bob vs Alice", "Total Goals", "Over 2.5", BetStatus.VOID),
    new Bet(98, 269, 2.3, "Alice", "David vs Eve", "1X2", "X", BetStatus.OPEN),
    new Bet(98, 269, 2.3, "Alice", "David vs Eve", "1X2", "X", BetStatus.WINNER),
    new Bet(99, 67, 3.49, "Alice", "Charlie vs David", "Handicap", "+1", BetStatus.OPEN),
    new Bet(99, 67, 3.49, "Alice", "Charlie vs David", "Handicap", "+1", BetStatus.LOSER),
    new Bet(100, 109, 2.06, "Charlie", "Eve vs Bob", "Handicap", "0", BetStatus.OPEN),
    new Bet(100, 109, 2.06, "Charlie", "Eve vs Bob", "Handicap", "0", BetStatus.VOID),
};

            var tasks = new List<Task>();

            foreach (var testBet in testBets)
            {
                betHandlingService.Handle(testBet);
            }

            await Task.Delay(50);

            betHandlingService.ShutDown();
        }
    }
}
