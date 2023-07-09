using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hawaiian.Utilities
{
  /// <summary>
  ///   <para>Provides a method to match query text using a fuzzy search algorithm.</para>
  /// </summary>
  public static class FuzzySearch
  {
    public static bool FuzzyMatch(string pattern, string origin, List<int> matches = null)
    {
      long outScore = 0;
      return FuzzySearch.FuzzyMatch(pattern, origin, ref outScore, matches);
    }

    public static bool FuzzyMatch(
      string pattern,
      string origin,
      ref long outScore,
      List<int> matches = null)
    {
      string lowerInvariant;
      int length;
      int index1;
      int strN;
      using (new FuzzySearch.ScopedProfiler("[FM] Init"))
      {
        outScore = -100000L;
        matches?.Clear();
        if (string.IsNullOrEmpty(origin))
          return false;
        if (string.IsNullOrEmpty(pattern))
          return true;
        lowerInvariant = origin.ToLowerInvariant();
        pattern = pattern.ToLowerInvariant();
        length = pattern.Length;
        index1 = 0;
        int index2 = lowerInvariant.Length - 1;
        char ch1 = pattern[0];
        char ch2 = pattern[pattern.Length - 1];
        while (index1 < lowerInvariant.Length && (int) ch1 != (int) lowerInvariant[index1])
          ++index1;
        while (index2 >= 0 && (int) ch2 != (int) lowerInvariant[index2])
          --index2;
        int num = index2 + 1;
        strN = num - index1;
        if (strN < length)
          return false;
        int index3 = 0;
        for (int index4 = index1; index3 < length && index4 < num; ++index4)
        {
          if ((int) pattern[index3] == (int) lowerInvariant[index4])
            ++index3;
        }
        if (index3 < length)
          return false;
      }
      using (new FuzzySearch.ScopedProfiler("[FM] Body"))
      {
        using (FuzzySearch.FuzzyMatchData fuzzyMatchData = FuzzySearch.FuzzyMatchData.Request(strN, length))
        {
          int num1 = strN - length + 1;
          int val2 = 0;
          using (new FuzzySearch.ScopedProfiler("[FM] Match loop"))
          {
            for (int index5 = 0; index5 < length; ++index5)
            {
              int num2 = strN + 1;
              bool flag1 = true;
              int index6 = Math.Max(index5, val2);
              for (int index7 = num1 + index5; index6 < index7; ++index6)
              {
                if (lowerInvariant[index6] == '<')
                {
                  while (index6 < index7 && lowerInvariant[index6] != '>')
                    ++index6;
                }
                int index8 = index6 + index1;
                bool flag2 = false;
                if ((int) pattern[index5] == (int) lowerInvariant[index8])
                  flag2 = true;
                if (index6 >= fuzzyMatchData.matchData.GetLength(0) || index5 >= fuzzyMatchData.matchData.GetLength(1))
                  return false;
                fuzzyMatchData.matchData[index6, index5] = flag2;
                if (flag2)
                {
                  if (flag1)
                  {
                    num2 = index6;
                    flag1 = false;
                  }
                  fuzzyMatchData.matches_indx[index5].Add(new FuzzySearch.ScoreIndx()
                  {
                    i = index6,
                    score = 1,
                    prev_mi = -1
                  });
                }
              }
              if (flag1)
                return false;
              val2 = num2;
            }
          }
          // ISSUE: explicit non-virtual call
          int num3 = strN - (matches != null ? (matches.Count) : 0);
          using (new FuzzySearch.ScopedProfiler("[FM] Best score 0"))
          {
            for (int index9 = 0; index9 < fuzzyMatchData.matches_indx[0].Count; ++index9)
            {
              int i = fuzzyMatchData.matches_indx[0][index9].i;
              int index10 = index1 + i;
              int num4 = 100 + -1 * num3;
              int num5 = -5 * index10;
              if (num5 < -15)
                num5 = -15;
              int num6 = num4 + num5;
              if (index10 == 0)
              {
                num6 += 35;
              }
              else
              {
                char c1 = origin[index10];
                char c2 = origin[index10 - 1];
                if (char.IsUpper(c1) && !char.IsUpper(c2))
                  num6 += 30;
                else if (c2 == '_' || c2 == ' ')
                  num6 += 30;
              }
              fuzzyMatchData.matches_indx[0][index9] = new FuzzySearch.ScoreIndx()
              {
                i = i,
                score = num6,
                prev_mi = -1
              };
            }
          }
          using (new FuzzySearch.ScopedProfiler("[FM] Best score 1..pattern_n"))
          {
            for (int index11 = 1; index11 < length; ++index11)
            {
              for (int index12 = 0; index12 < fuzzyMatchData.matches_indx[index11].Count; ++index12)
              {
                FuzzySearch.ScoreIndx scoreIndx = fuzzyMatchData.matches_indx[index11][index12];
                int index13 = index1 + fuzzyMatchData.matches_indx[index11][index12].i;
                char c3 = origin[index13];
                char c4 = origin[index13 - 1];
                if (char.IsUpper(c3) && !char.IsUpper(c4))
                  scoreIndx.score += 30;
                else if (c4 == '_' || c4 == ' ')
                  scoreIndx.score += 30;
                int num7 = 0;
                int num8 = -1;
                for (int index14 = 0; index14 < fuzzyMatchData.matches_indx[index11 - 1].Count; ++index14)
                {
                  int i = fuzzyMatchData.matches_indx[index11 - 1][index14].i;
                  if (i < scoreIndx.i)
                  {
                    int score = fuzzyMatchData.matches_indx[index11 - 1][index14].score;
                    if (i == scoreIndx.i - 1)
                      score += 75;
                    if (num8 < score)
                    {
                      num8 = score;
                      num7 = index14;
                    }
                  }
                  else
                    break;
                }
                scoreIndx.score += num8;
                scoreIndx.prev_mi = num7;
                fuzzyMatchData.matches_indx[index11][index12] = scoreIndx;
              }
            }
          }
          int index15 = 0;
          int index16 = length - 1;
          for (int index17 = 1; index17 < fuzzyMatchData.matches_indx[index16].Count; ++index17)
          {
            if (fuzzyMatchData.matches_indx[index16][index15].score < fuzzyMatchData.matches_indx[index16][index17].score)
              index15 = index17;
          }
          FuzzySearch.ScoreIndx scoreIndx1 = fuzzyMatchData.matches_indx[index16][index15];
          outScore = (long) scoreIndx1.score;
          if (matches != null)
          {
            using (new FuzzySearch.ScopedProfiler("[FM] Matches calc"))
            {
              matches.Capacity = length;
              matches.Add(scoreIndx1.i + index1);
              int prevMi = scoreIndx1.prev_mi;
              for (int index18 = length - 2; index18 >= 0; --index18)
              {
                matches.Add(fuzzyMatchData.matches_indx[index18][prevMi].i + index1);
                prevMi = fuzzyMatchData.matches_indx[index18][prevMi].prev_mi;
              }
              matches.Reverse();
            }
          }
          return true;
        }
      }
    }

    private struct ScoreIndx
    {
      public int i;
      public int score;
      public int prev_mi;
    }

    private class FuzzyMatchData : IDisposable
    {
      public List<FuzzySearch.ScoreIndx>[] matches_indx;
      public bool[,] matchData;

      private FuzzyMatchData(int strN, int patternN)
      {
        this.matchData = new bool[strN, patternN];
        this.matches_indx = new List<FuzzySearch.ScoreIndx>[patternN];
        for (int index = 0; index < patternN; ++index)
          this.matches_indx[index] = new List<FuzzySearch.ScoreIndx>(8);
      }

      public void Dispose()
      {
      }

      public static FuzzySearch.FuzzyMatchData Request(int strN, int patternN) => new FuzzySearch.FuzzyMatchData(strN, patternN);
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    internal struct ScopedProfiler : IDisposable
    {
      public ScopedProfiler(string name)
      {
      }

      public ScopedProfiler(string name, UnityEngine.Object targetObject)
      {
      }

      public void Dispose()
      {
      }
    }
  }
}