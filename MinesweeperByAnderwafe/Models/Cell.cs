using System.Dynamic;

namespace MinesweeperByAnderwafe.Models;

public class Cell
{
    private string _name;
    private char _value;

    public Cell(string name, char value = ' ')
    {
        _name = name;
        _value = value;
    }
    
    public string Name
    {
        get => _name;
    }

    public char Value
    {
        get => _value;
    }

    void Open()
    {
        
    }
}