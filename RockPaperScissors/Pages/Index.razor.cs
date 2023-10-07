namespace RockPaperScissors.Pages
{
    public partial class Index
    {
        //menu screens
        private bool menuScreen = true;
        private bool hideMenus = false;

        //game modes
        private bool normalGameMode = false;
        private bool bbtGameMode = false;
        private bool showScoreBoard = false;

        public int userScore = 0;
        public int botScore = 0;


        private void normalGameModeClicked()
        {
            normalGameMode = true;
            hideMenus = true;
            StateHasChanged();
        }

        private void BackButtonFromNormalModeClicked()
        {
            normalGameMode = false;
            hideMenus = false;
            StateHasChanged();
        }

        private void bbtGameModeClicked()
        {
            bbtGameMode = true;
            hideMenus = true;
            StateHasChanged();
        }

        private void BackButtonFromBBTModeClicked()
        {
            bbtGameMode = false;
            hideMenus = false;
            StateHasChanged();
        }

        private void showScore()
        {
            showScoreBoard = true;
            hideMenus = true;
        }

        private void ShowGameModesMenus()
        {
            showScoreBoard = false;
            hideMenus = false;
        }

        private void UpdateScore(bool isUserWon)
        {
            if (isUserWon)
            {
                userScore++;
            }
            else
            {
                botScore++;
            }
        }
    }
}
