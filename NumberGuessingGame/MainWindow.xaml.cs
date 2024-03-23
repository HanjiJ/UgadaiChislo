using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NumberGuessingGame
{
    public partial class MainWindow : Window
    {
        private int secretNumber;
        private int currentPlayer = 1;
        private int player1Score = 0;
        private int player2Score = 0;
        private int maxScore = 3;
        private int minRange;
        private int maxRange;
        private Random rnd = new Random();
        private string player1Name;
        private string player2Name;


        public MainWindow()
        {
            InitializeComponent();
            btnNewGame.IsEnabled = false;
            txtGuess.IsEnabled = false;
            btnGuess.IsEnabled = false;
        }

        private void NewGame()
        {
            currentPlayer = 1;
            player1Score = 0;
            player2Score = 0;
            UpdateScores();
            GenerateSecretNumber();
            MessageBox.Show("Компьютер загадал число.");
            btnNewGame.IsEnabled = true;
            btnSetRange.IsEnabled = false;
            txtMin.IsEnabled = false;
            txtMax.IsEnabled = false;
            txtGuess.IsEnabled = true;
            btnGuess.IsEnabled = true;
            lblTurn.Content = $"Ход: {player1Name}";
        }

        private void GenerateSecretNumber()
        {
            secretNumber = rnd.Next(minRange, maxRange + 1);
        }

        private void UpdateScores()
        {
            lblScore.Content = $"Счёт: {txtPlayer1Name.Text} - {player1Score}, {txtPlayer2Name.Text} - {player2Score}";
        }

        private void CheckGuess(int guess)
        {
            if (guess == secretNumber)
            {
                // Определяем имя текущего игрока
                string playerName = (currentPlayer == 1) ? txtPlayer1Name.Text : txtPlayer2Name.Text;

                MessageBox.Show($"Игрок {playerName} угадал число!");

                // Увеличиваем счет соответствующего игрока
                if (currentPlayer == 1)
                {
                    player1Score++;
                }
                else
                {
                    player2Score++;
                }

                // Проверяем, достиг ли кто-то из игроков максимального счета
                if (player1Score == maxScore || player2Score == maxScore)
                {
                    // Определяем имя победителя
                    string winnerName = (player1Score == maxScore) ? txtPlayer1Name.Text : txtPlayer2Name.Text;

                    MessageBox.Show($"Победил игрок {winnerName}!");

                    // Отключаем кнопку начала новой игры и включаем кнопку установки диапазона чисел
                    btnNewGame.IsEnabled = false;
                    btnSetRange.IsEnabled = true;

                    // Включаем поля ввода минимального и максимального значения
                    txtMin.IsEnabled = true;
                    txtMax.IsEnabled = true;

                    // Отключаем поле ввода числа для угадывания и кнопку "Угадать"
                    txtGuess.IsEnabled = false;
                    btnGuess.IsEnabled = false;
                }
                else
                {
                    // Переключаем игрока и генерируем новое секретное число
                    currentPlayer = (currentPlayer == 1) ? 2 : 1;
                    GenerateSecretNumber();
                    MessageBox.Show("Новая игра! Компьютер загадал новое число.");
                }

                // Обновляем отображение счета
                UpdateScores();
            }
            else if (guess < secretNumber)
            {
                MessageBox.Show("Не угадал.");
            }
        }


        private void btnGuess_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtGuess.Text, out int guess))
            {
                CheckGuess(guess);

                // После каждого хода меняем игрока
                currentPlayer = (currentPlayer == 1) ? 2 : 1;

                // Обновляем отображение текущего игрока
                lblTurn.Content = $"Ход: Игрок {currentPlayer}";

                // Очищаем содержимое TextBox
                txtGuess.Text = "";
            }
            else
            {
                MessageBox.Show("Введите корректное число!");
            }
            lblTurn.Content = $"Ход: {(currentPlayer == 1 ? player1Name : player2Name)}";

        }



        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            txtMin.Text = ""; // Очищаем поле ввода минимального значения
            txtMax.Text = ""; // Очищаем поле ввода максимального значения
            btnSetRange.IsEnabled = true; // Включаем кнопку "Установить"
            txtMin.IsEnabled = true; // Включаем поле ввода минимального значения
            txtMax.IsEnabled = true; // Включаем поле ввода максимального значения
            btnNewGame.IsEnabled = false; // Отключаем кнопку "Начать игру"
            txtGuess.IsEnabled = false; // Отключаем поле ввода числа для угадывания
            btnGuess.IsEnabled = false; // Отключаем кнопку "Угадать"
            lblScore.Content = "Счёт:"; // Сбрасываем отображение счёта
        }

        private void btnSetRange_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что введены корректные данные
            if (int.TryParse(txtMin.Text, out minRange) && int.TryParse(txtMax.Text, out maxRange) && minRange < maxRange)
            {
                // Устанавливаем имя первого игрока
                lblTurn.Content = $"Ход: {txtPlayer1Name.Text}";

                // Устанавливаем имя второго игрока
                lblTurn.Content = $"Ход: {txtPlayer2Name.Text}";

                // Запускаем новую игру
                NewGame();
            }
            else
            {
                MessageBox.Show("Введите корректный диапазон чисел!");
            }
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, является ли вводимый символ числом
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // Если это не число, отменяем ввод
            }
            else if (string.IsNullOrEmpty(textBox.Text + e.Text)) // Проверяем, что вводимое значение не является пустым
            {
                // Пропускаем проверку для пустого значения
            }
            else
            {
                // Проверяем, входит ли новое значение в заданный диапазон
                int value;
                if (int.TryParse(textBox.Text + e.Text, out value))
                {
                    if (value < minRange || value > maxRange)
                    {
                        MessageBox.Show("Введите число в диапазоне от " + minRange + " до " + maxRange);
                        e.Handled = true;
                    }
                }
            }
        }

        private void NumericTextBox_PreviewTextInput1(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли вводимый символ числом
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // Если это не число, отменяем ввод
            }
        }
        private void btnApplyName_Click(object sender, RoutedEventArgs e)
        {
            player1Name = txtPlayer1Name.Text;
            player2Name = txtPlayer2Name.Text;
        }
    }
}