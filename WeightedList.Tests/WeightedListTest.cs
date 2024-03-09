using System.Collections.Generic;

namespace WeightedList.Tests;

public class WeightedListTest
{
    [Fact]
    public void Test1()
    {
        WeightedList<int> list = new(0)
        {
            {9, 9f},
            {2, 2f},
            {5, 5f},
            {4, 4f},
            {7, 7f},
            {8, 8f},
            {3, 3f},
            {10, 10f},
            {6, 6f},
            {1, 1f},
        };

        Assert.Equal(10, list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            Assert.Equal(i + 1, list[i].item);
        }


        Dictionary<int, int> dict = new();
        for (int i = 0; i < 100; i++)
        {
            var item = list.GetRandom();
            if (dict.ContainsKey(item))
            {
                dict[item]++;
            }
            else
            {
                dict.Add(item, 1);
            }
        }

    }
}