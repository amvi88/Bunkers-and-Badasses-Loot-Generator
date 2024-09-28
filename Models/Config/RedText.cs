using Models.Common;

public class RedText : SpecialText
{
    public Element? Element {get; set;}
}

public class TealText : SpecialText
{
}

public class SpecialText
{
    public string Name { get; set; }
    public string Effect { get; set; }
}
