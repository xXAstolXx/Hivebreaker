using CSVImporter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TwoWayOMatData : BaseImportObject
{
    public string statName;

    public int maxHPPlus;
    public int maxHPMinus;

    public int meleeAttackPlus;
    public int meleeAttackMinus;
    public int rangedAttackPlus;
    public int rangedAttackMinus;


    public int meleeDefensePlus;
    public int meleeDefenseMinus;
    public int rangedDefensePlus;
    public int rangedDefenseMinus;

    public float moveSpeedPlus;
    public float moveSpeedMinus;

    public int meleeAttackSpeedPlus;
    public int meleeAttackSpeedMinus;
    public int rangedAttackSpeedPlus;
    public int rangedAttackSpeedMinus;

    public float bulletSpeedPlus;
    public float bulletSpeedMinus;

    public override void SetupFromTokens(string[] tokens)
    {
        try
        {
            AssertRowLength(tokens.Length, 19);

            statName = tokens[0];

            maxHPPlus = int.Parse(tokens[1]);
            maxHPMinus = int.Parse(tokens[2]);

            meleeAttackPlus = int.Parse(tokens[3]);
            meleeAttackMinus = int.Parse(tokens[4]);

            rangedAttackPlus = int.Parse(tokens[5]);
            rangedAttackMinus = int.Parse(tokens[6]);

            meleeDefensePlus = int.Parse(tokens[7]);
            meleeDefenseMinus = int.Parse(tokens[8]);

            rangedDefensePlus = int.Parse(tokens[9]);
            rangedDefenseMinus = int.Parse(tokens[10]);

            moveSpeedPlus = float.Parse(tokens[11], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
            moveSpeedMinus = float.Parse(tokens[12], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));

            meleeAttackSpeedPlus = int.Parse(tokens[13]);
            meleeAttackSpeedMinus = int.Parse(tokens[14]);

            rangedAttackSpeedPlus = int.Parse(tokens[15]);
            rangedAttackSpeedMinus = int.Parse(tokens[16]);

            bulletSpeedPlus = float.Parse(tokens[17], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
            bulletSpeedMinus = float.Parse(tokens[18], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
        }
        catch 
        {
            throw new Exception("Cant Setup data because of to much or less tokens");
        }
    }

}
