using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringSimilarity
{
    public static bool IsSimilar(string s1, string s2, float threshold = 0.3f)
    {
        return GetLevenshteinDistanceRatio(s1, s2) <= threshold;
    }

    public static float GetLevenshteinDistanceRatio(string s1, string s2)
    {
        int levenshteinDistance = GetLevenshteinDistance(s1, s2);
        
        int denominator = Mathf.Max(Normalize(s1).Length, Normalize(s2).Length);

        if (denominator == 0) return 0f;
        
        return (float)levenshteinDistance / denominator;
    }

    public static string Normalize(string s)
    {
        if (s == null) return string.Empty;
        string t = Regex.Replace(s, "[ .!?,\"-]", "").ToLowerInvariant();
        return t;
    }

    public static int GetLevenshteinDistance(string firstString, string secondString)
    {
        string normalizedFirstString = Normalize(firstString);
        string normalizedSecondString = Normalize(secondString);
        
        int firstLength = normalizedFirstString.Length;
        int secondLength = normalizedSecondString.Length;
        
        if (firstLength == 0) return secondLength;
        if (secondLength == 0) return firstLength;

        int[][] matrix = new int[firstLength + 1][];
        for (int index = 0; index < firstLength + 1; index++)
        {
            matrix[index] = new int[secondLength + 1];
        }

        for (int i = 0; i <= firstLength; i++)
        {
            matrix[i][0] = i;
        }

        for (int j = 0; j <= secondLength; j++)
        {
            matrix[0][j] = j;
        }

        for (int j = 1; j <= secondLength; j++)
        {
            for (int i = 1; i <= firstLength; i++)
            {
                int substitutionCost = (normalizedFirstString[i - 1] == normalizedSecondString[j - 1]) ? 0 : 1;

                int deletion = matrix[i - 1][j] + 1;
                int insertion = matrix[i][j - 1] + 1;
                int substitution = matrix[i - 1][j - 1] + substitutionCost;
                
                int[] nums = { deletion, insertion, substitution };
                 
                matrix[i][j] = Mathf.Min(nums);
            }
        }
        
        return matrix[firstLength][secondLength];
    }
}
