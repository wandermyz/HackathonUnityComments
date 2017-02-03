using System.Collections.Generic;

public class DummyData
{
    public static List<Comment> getDummyData()
    {
        return new List<Comment>{
            new Comment(
                "Hello World",
                0f,
                0f
			),
            new Comment(
                "Hell",
                0.1f,
                0.1f
            ),
        };
    }
}