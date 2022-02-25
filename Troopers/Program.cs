using System;
using System.Threading;

namespace Troopers
{
    static class Program
    {
        private static string _opponentAttackCoords;
        private static int _ownedLands;
        private static int _opponentLands;
        private static int _emptyLands;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once InconsistentNaming
        private static Game game = new Game
        {
            Title = "Troopers",
            Map = GenerateMap(),
            State = "preInit"
        };

        public static void Main()
        {
            foreach (var land in game.Map)
            {
                if (land == 0)
                {
                    _emptyLands++;
                }

                if (land == 10)
                {
                    _ownedLands++;
                }

                if (land == 01)
                {
                    _opponentLands++;
                }
            }

            Run();
        }

        private static void Run()
        {
            Utils.Log("Welcome to Troopers.");
            Utils.Log("Your mission is to predict which place of the map has troops on it and not.");
            Utils.Log("By your predictions soviet army will strike your coordinates.");
            Utils.Log("--------------------");
            Utils.Log($"You Own {_ownedLands} lands.");
            GameLoop();
        }

        private static void GameLoop()
        {
            while (true)
            {
                if (_ownedLands == 100)
                {
                    Utils.Log("You Win!");
                    break;
                }

                if (_ownedLands == 0)
                {
                    Utils.Log("You Loose!");
                    break;
                }

                if (game.GetState() == "opponentAttackSuccess")
                {
                    Utils.Log(
                        $"Intelligence: Opponent has successfully gained control on {_opponentAttackCoords} coordinates.");
                    game.ChangeState("gameLoop");
                }

                Utils.Log("Operations;");
                Utils.Log("\tsearch: Launch intelligence search on coordinates");
                Utils.Log("\tattack: Launch attack on coordinates");
                Utils.Log("\tstatus: See status of your soldiers");
                Utils.Log("\tquit: Quit game.");
                Utils.Log("Select Option> ", false);
                switch (Utils.Read())
                {
                    case "search":
                        SearchMenu();
                        break;
                    case "attack":
                        AttackMenu();
                        break;
                    case "status":
                        ShowStatus();
                        break;
                    case "quit":
                        Quit();
                        break;
                }
            }
        }

        private static void SearchMenu()
        {
            Utils.Log("Search Coordinates (Leave Empty To Cancel)> ", false);
            var coords = Utils.Read();

            if (string.IsNullOrEmpty(coords))
            {
                Console.Clear();
                GameLoop();
            }

            string[] tokens = coords!.Split('x');

            if (tokens.Length <= 1)
            {
                tokens[1] = 0.ToString();
            }

            if (!int.TryParse(tokens[0], out var x))
            {
                Utils.Log("Warning! You can enter only numbers between 0 to 9");
                SearchMenu();
            }

            if (!int.TryParse(tokens[1], out var y))
            {
                Utils.Log("Warning! You can enter only numbers between 0 to 9");
                SearchMenu();
            }

            if (x > 9)
            {
                Utils.Log("Warning! You can enter only numbers between 0 to 9");
                SearchMenu();
            }

            if (y > 9)
            {
                Utils.Log("Warning! You can enter only numbers between 0 to 9");
                SearchMenu();
            }

            Utils.Slog($"Intelligence: Gathering information about {coords} coordinates");
            Utils.Slog("Intelligence: Sending discovery planes...");
            Utils.Slog("Intelligence: Gathering information from active agents...");
            Utils.Slog("Intelligence: ...");
            Utils.Slog("Intelligence: ...");
            if (game.Map[x, y] != 10)
            {
                Random random = new Random();
                if (random.Next(2) == 1)
                {
                    Utils.Log("Intelligence: Our agents and discovery planes has reported the land is empty!");
                    Thread.Sleep(2000);
                }
                else
                {
                    Utils.Log(
                        "Intelligence: Our agents and discovery planes has reported nothing! Maybe there's a leak?");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Utils.Log("Intelligence: Looks like given coordinates are full with our troops!");
            }
        }

        private static void AttackMenu()
        {
            Utils.Log("Attack Coordinates (Leave Empty To Cancel)> ", false);
            var coords = Utils.Read();

            if (string.IsNullOrEmpty(coords))
            {
                Console.Clear();
                GameLoop();
            }

            string[] tokens = coords!.Split('x');

            if (tokens.Length <= 1)
            {
                tokens[1] = 0.ToString();
            }

            if (!int.TryParse(tokens[0], out var x))
            {
                Utils.Log("Warning! You can enter only numbers between 0 to 9");
                AttackMenu();
            }

            if (!int.TryParse(tokens[1], out var y))
            {
                Utils.Log("Warning! You can enter only numbers between 0 to 9");
                AttackMenu();
            }

            if (x > 9)
            {
                Utils.Log("Warning! You can enter only numbers between 0 to 9");
                AttackMenu();
            }

            if (y > 9)
            {
                Utils.Log("Warning! You can enter only numbers between 0 to 9");
                AttackMenu();
            }

            Utils.Slog($"Intelligence: Gathering information about {coords} coordinates");

            game.ChangeState("attack");
            if (game.Map[x, y] != 10)
            {
                Random random = new Random();
                if (random.Next(2) == 1)
                {
                    if (game.Map[x, y] == 01)
                    {
                        _opponentLands--;
                        _ownedLands++;
                    }
                    else
                    {
                        _emptyLands--;
                        _ownedLands++;
                    }

                    game.Map[x, y] = 10;
                    Utils.Slog("Attack Status> Sending troops...");
                    Utils.Slog("Attack Status> ...");
                    Utils.Slog("Attack Status> ...");
                    Utils.Log("Attack Status> Success!");
                    Thread.Sleep(2000);
                }
                else
                {
                    Utils.Slog("Attack Status> Sending troops...");
                    Utils.Slog("Attack Status> ...");
                    Utils.Slog("Attack Status> ...");
                    Utils.Log("Attack Status> Failed!");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Utils.Log("Intelligence: Looks like given coordinates are full with our troops!");
            }

            OpponentAttack();
            Thread.Sleep(2000);
            Console.Clear();
        }

        private static void OpponentAttack()
        {
            Random random = new Random();
            if (random.Next(2) == 1)
            {
                game.ChangeState("opponentAttack");
                var x = random.Next(0, 9);
                var y = random.Next(0, 9);
                _opponentAttackCoords = $"{x}x{y}";
                Utils.Log($"Intelligence: Opponent is launching an attack on {_opponentAttackCoords} coordinates.");
                if (random.Next(2) == 1)
                {
                    if (game.Map[x, y] != 01)
                    {
                        if (game.Map[x, y] == 10)
                        {
                            _opponentLands++;
                            _ownedLands--;
                        }
                        else
                        {
                            _emptyLands--;
                            _opponentLands++;
                        }

                        game.Map[x, y] = 10;
                        game.ChangeState("opponentAttackSuccess");
                    }
                    else
                    {
                        Utils.Log($"Intelligence: Opponent has failed attack on {_opponentAttackCoords} coordinates.");
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    Utils.Log($"Intelligence: Opponent has failed attack on {_opponentAttackCoords} coordinates.");
                    Thread.Sleep(2000);
                }
            }
        }

        private static void ShowStatus()
        {
            Utils.Log($"You Own {_ownedLands} lands.");
            Utils.Log($"You need to attack {_opponentLands + _emptyLands} lands.");
        }

        private static void Quit()
        {
            Environment.Exit(1);
        }

        private static int[,] GenerateMap()
        {
            Random random = new Random();
            var objects = new[] {10, 01, 0};
            // 0 means empty
            // 01 means filled with opponent's troops
            // 10 means filled with your troops
            return new[,]
            {
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
                {
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)],
                    objects[random.Next(0, objects.Length)], objects[random.Next(0, objects.Length)]
                },
            };
        }
    }
}