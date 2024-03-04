//using CSVImporter;
using CSVImporter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class OneWayOMatData : BaseImportObject
{
    public string statName;

    public int maxHP;

    public int meleeAttack;
    public int rangedAttack;

    public int meleeDefense;
    public int rangedDefense;

    public float moveSpeed;

    public int meleeAttackSpeed;
    public int rangedAttackSpeed;

    public float bulletSpeed;

    public override void SetupFromTokens(string[] tokens)
    {
        try
        {
            AssertRowLength(tokens.Length, 10);

            statName = tokens[0];
            maxHP = int.Parse(tokens[1]);
            meleeAttack = int.Parse(tokens[2]);
            rangedAttack = int.Parse(tokens[3]);
            meleeDefense = int.Parse(tokens[4]);
            rangedDefense = int.Parse(tokens[5]);
            moveSpeed = float.Parse(tokens[6], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
            meleeAttackSpeed = int.Parse(tokens[7]);
            rangedAttackSpeed = int.Parse(tokens[8]);
            bulletSpeed = float.Parse(tokens[9], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
        }
        catch 
        {
            throw new Exception("Cant Setup data because of to much or less tokens");
        }
    }

}
