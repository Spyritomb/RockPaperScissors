using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RockPaperScissors.Shared.Constants;

namespace RockPaperScissors.Shared.GameModes
{
    public partial class RPSBigBangTheory
    {
        [Inject]
        private IJSRuntime _jsInterop { get; set; }

        [Parameter]
        public EventCallback NavigateBackButtonCallback { get; set; }
        [Parameter]
        public EventCallback<bool> UpdateScoreCallback { get; set; }

        private string userHand = string.Empty;
        private string userHandPrevious = string.Empty;
        private string botHandFinal = string.Empty;

        public bool userRockSelected;
        public bool userPaperSelected;
        public bool userScissorsSelected;
        public bool userLizardSelected;
        public bool userSpokSelected;
        public bool showModal;

        public bool isCheckBoxCheck = false;

        private string currentWord = string.Empty;
        private bool isAnimating = false;


        private bool _isSomethingWentWrong;
        private bool isSomethingWentWrong
        {
            get
            {
                return this._isSomethingWentWrong;
            }
            set
            {
                this._isSomethingWentWrong = value;
                StateHasChanged();
            }
        }


        private bool _isTie;
        private bool isTie
        {
            get
            {
                return this._isTie;
            }
            set
            {
                this._isTie = value;
                StateHasChanged();
            }
        }

        private bool _isUserWon;
        private bool isUserWon
        {
            get
            {
                return this._isUserWon;
            }
            set
            {
                this._isUserWon = value;
                StateHasChanged();
            }
        }

        private bool _isBotWon;
        private bool isBotWon
        {
            get
            {
                return this._isBotWon;
            }
            set
            {
                this._isBotWon = value;
                StateHasChanged();
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }


        protected void StartGame()
        {
            string botHand = GetBotHand();
            string userHandOption = userHand;
            CalculateOutcome(userHandOption, botHand);
        }

        protected string GetBotHand()
        {
            Console.Write(userHandPrevious);
            if (isCheckBoxCheck)
            {
                return userHandPrevious;
            }
            else
            {
                Random random = new Random();
                int botHand = random.Next(3);
                switch (botHand)
                {
                    case 0:
                        botHandFinal = AppConstants.GameHandOptions.ROCK;
                        return botHandFinal;
                    case 1:
                        botHandFinal = AppConstants.GameHandOptions.PAPER;
                        return botHandFinal;
                    case 2:
                        botHandFinal = AppConstants.GameHandOptions.SCISSORS;
                        return botHandFinal;
                    case 3:
                        botHandFinal = AppConstants.GameHandOptions.LIZARD;
                        return botHandFinal;
                    default:
                        botHandFinal = AppConstants.GameHandOptions.SPOK;
                        return botHandFinal;
                }
            }
        }

        protected void ChangeUserHand(bool? rock = false, bool? paper = false, bool? scissor = false, bool? lizard = false, bool? spok = false)
        {
            //could be done with a switch instead
            if (rock.HasValue && rock.Value)
            {
                userHand = AppConstants.GameHandOptions.ROCK;
                if (userRockSelected)
                {
                    userRockSelected = false;
                    userHand = string.Empty;
                }
                else
                {
                    userRockSelected = true;
                }
                userPaperSelected = false;
                userScissorsSelected = false;
                userLizardSelected = false;
                userSpokSelected = false;
                StateHasChanged();
                return;
            }
            if (paper.HasValue && paper.Value)
            {
                userHand = AppConstants.GameHandOptions.PAPER;
                if (userPaperSelected)
                {
                    userPaperSelected = false;
                    userHand = string.Empty;
                }
                else
                {
                    userPaperSelected = true;
                }
                userRockSelected = false;
                userScissorsSelected = false;
                userLizardSelected = false;
                userSpokSelected = false;
                StateHasChanged();
                return;
            }
            if (scissor.HasValue && scissor.Value)
            {
                userHand = AppConstants.GameHandOptions.SCISSORS;
                if (userScissorsSelected)
                {
                    userScissorsSelected = false;
                    userHand = string.Empty;
                }
                else
                {
                    userScissorsSelected = true;
                }
                userRockSelected = false;
                userPaperSelected = false;
                userLizardSelected = false;
                userSpokSelected = false;
                StateHasChanged();
                return;
            }

            if (lizard.HasValue && lizard.Value)
            {
                userHand = AppConstants.GameHandOptions.LIZARD;
                if (userLizardSelected)
                {
                    userLizardSelected = false;
                    userHand = string.Empty;
                }
                else
                {
                    userLizardSelected = true;
                }
                userRockSelected = false;
                userPaperSelected = false;
                userScissorsSelected = false;
                userSpokSelected = false;
                StateHasChanged();
                return;
            }

            if (spok.HasValue && spok.Value)
            {
                userHand = AppConstants.GameHandOptions.SPOK;
                userHand = AppConstants.GameHandOptions.LIZARD;
                if (userSpokSelected)
                {
                    userSpokSelected = false;
                    userHand = string.Empty;
                }
                else
                {
                    userSpokSelected = true;
                }
                userRockSelected = false;
                userPaperSelected = false;
                userScissorsSelected = false;
                userLizardSelected = false;
                StateHasChanged();
                return;
            }

        }
        protected async void CalculateOutcome(string userHand, string botHand)
        {
            if (string.IsNullOrEmpty(userHand) || string.IsNullOrEmpty(botHand))
            {
                await _jsInterop.InvokeVoidAsync("showChooseHandToast");
                isSomethingWentWrong = true;
                isTie = false;
                isUserWon = false;
                isBotWon = false;
                return;
            }
            userHandPrevious = userHand;
            if (userHand.Equals(botHand))
            {
                isTie = true;
                isUserWon = false;
                isBotWon = false;
                isSomethingWentWrong = false;
                return;
            }

            if ((userHand.Equals(AppConstants.GameHandOptions.ROCK) && (botHand.Equals(AppConstants.GameHandOptions.SCISSORS) || botHand.Equals(AppConstants.GameHandOptions.LIZARD))) ||
                (userHand.Equals(AppConstants.GameHandOptions.PAPER) && (botHand.Equals(AppConstants.GameHandOptions.ROCK) || botHand.Equals(AppConstants.GameHandOptions.SPOK))) ||
                (userHand.Equals(AppConstants.GameHandOptions.SCISSORS) && (botHand.Equals(AppConstants.GameHandOptions.PAPER) || botHand.Equals(AppConstants.GameHandOptions.LIZARD))) ||
                (userHand.Equals(AppConstants.GameHandOptions.LIZARD) && (botHand.Equals(AppConstants.GameHandOptions.PAPER) || botHand.Equals(AppConstants.GameHandOptions.SPOK))) ||
                (userHand.Equals(AppConstants.GameHandOptions.SPOK) && (botHand.Equals(AppConstants.GameHandOptions.SCISSORS) || botHand.Equals(AppConstants.GameHandOptions.ROCK)))
                )
            {
                isUserWon = true;
                isTie = false;
                isBotWon = false;
                isSomethingWentWrong = false;
                if (UpdateScoreCallback.HasDelegate)
                {
                    await UpdateScoreCallback.InvokeAsync(true);
                }
                return;
            }
            else
            {
                isBotWon = true;
                isUserWon = false;
                isTie = false;
                isSomethingWentWrong = false;
                if (UpdateScoreCallback.HasDelegate)
                {
                    await UpdateScoreCallback.InvokeAsync(false);
                }
                return;
            }
        }



        protected async Task AnimateWordsAndShowResult()
        {
            if (string.IsNullOrEmpty(userHand))
            {
                await _jsInterop.InvokeVoidAsync("showChooseHandToast");
                return;
            }
            if (isAnimating)
            {
                return;
            }

            if (string.IsNullOrEmpty(userHand))
            {
                await _jsInterop.InvokeVoidAsync("showChooseHandToast");
                return;
            }
            isAnimating = true;
            var words = new List<string> { "Rock,", "Paper,", "Scissors,", "Lizard", "Spok", "Go!" };

            foreach (var word in words)
            {
                currentWord = word;
                StateHasChanged();
                await Task.Delay(500);
            }

            StartGame();
            isAnimating = false;
        }

        protected void showHowToModal()
        {
            showModal = true;
        }
        protected void hideHowToModal()
        {
            showModal = false;
        }

        protected async Task NavigateBack()
        {
            if (NavigateBackButtonCallback.HasDelegate)
            {
                await NavigateBackButtonCallback.InvokeAsync();
            }
        }

    }
}

