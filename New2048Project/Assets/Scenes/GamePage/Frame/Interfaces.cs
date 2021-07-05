
public interface GameController
{
    public bool IsPause();
    public void GameStart();
    public void GamePause(bool isPause);
    public void GameOver();
    public void ExitGame();
    public void SaveGame();
    public void LoadGame();
    public void SaveBestScore();
    public void LoadBestScore();
}

public interface UserInput
{
    public void PCInput();
    public void PhoneInput();
}


