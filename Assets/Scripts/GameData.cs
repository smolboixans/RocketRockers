using SQLite;

public class GameData {
    [PrimaryKey, AutoIncrement]
    public int PlayerID {get; set;}
    public string PlayerName {get; set;}
    public int Score {get; set;}

}
