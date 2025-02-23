using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static int CalculateSqrt(int value)
    {
        if (value < 0) throw new ArgumentException($"{value} is a negative number");

        float tolerance = 0.1f;             // 오차 범위
        float guessValue = value / 2.0f;    // 초기 추정값

        // 뉴턴-랩슨 방법을 사용한 반복 계산
        while (CalculateAbs(guessValue * guessValue - value) > tolerance)
        {
            guessValue = (guessValue + value / guessValue) / 2.0f;
        }

        int result = CalculateRound(guessValue);

        if (result * result != value) throw new AggregateException($"{value} does not have a integer square root");

        return result;
    }

    public static int CalculateRound(float value) => (int)(value + 0.5f);
    public static float CalculateAbs(float value) => value < 0.0f ? -value : value;
    public static int CalculateRoundDownToUnit(float value, int unit) => (int)(value - (value % unit)) - (value % unit < 0 ? unit : 0);
    public static Vector3Int CalculateRoundDownToUnitV3(Vector3 value, int unit) => new Vector3Int
        (CalculateRoundDownToUnit(value.x, unit),
         CalculateRoundDownToUnit(value.y, unit),
         CalculateRoundDownToUnit(value.z, unit));
}
