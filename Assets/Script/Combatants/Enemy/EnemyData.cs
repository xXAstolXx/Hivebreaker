//using CSVImporter;
using CSVImporter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EnemyData : BaseImportObject
{
    /////////////////////////
    // Base Stats          //
    /////////////////////////

    public string enemyName;

    public int maxHP = 0;

    public int meleeAttack = 0;
    public int rangedAttack = 0;

    public int meleeDefense = 0;
    public int rangedDefense = 0;

    public float moveSpeed = 0.0f;

    public int meleeAttackSpeed = 0;
    public int rangedAttackSpeed = 0;

    public float bulletSpeed = 0.0f;

    /////////////////////////
    // Capsule Increase    //
    /////////////////////////
    
    public int capsuleMaxHP = 0;

    public int capsuleMeleeAttack = 0;
    public int capsuleRangedAttack = 0;
    
    public int capsuleMeleeDefense = 0;
    public int capsuleRangedDefense = 0;

    public float capsuleMoveSpeed = 0.0f;

    public int capsuleMeleeAttackSpeed = 0;
    public int capsuleRangedAttackSpeed = 0;

    public float capsuleBulletSpeed = 0.0f;

    /////////////////////////

    public override void SetupFromTokens(string[] tokens)
    {
        try
        {
            AssertRowLength(tokens.Length, 19);

            enemyName = tokens[0];
            maxHP = int.Parse(tokens[1]);
            meleeAttack = int.Parse(tokens[2]);
            rangedAttack = int.Parse(tokens[3]);
            meleeDefense = int.Parse(tokens[4]);
            rangedDefense = int.Parse(tokens[5]);
            moveSpeed = float.Parse(tokens[6], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
            meleeAttackSpeed = int.Parse(tokens[7]);
            rangedAttackSpeed = int.Parse(tokens[8]);
            bulletSpeed = float.Parse(tokens[9], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
            
            // Capsule Increase
            capsuleMaxHP = int.Parse(tokens[10]);
            capsuleMeleeAttack = int.Parse(tokens[11]);
            capsuleRangedAttack = int.Parse(tokens[12]);
            capsuleMeleeDefense = int.Parse(tokens[13]);
            capsuleRangedDefense = int.Parse(tokens[14]);
            capsuleMoveSpeed = float.Parse(tokens[15], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
            capsuleMeleeAttackSpeed = int.Parse(tokens[16]);
            capsuleRangedAttackSpeed = int.Parse(tokens[17]);
            capsuleBulletSpeed = float.Parse(tokens[18], System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));
        }
        catch 
        {
            throw new Exception("Cant Setup data because of to much or less tokens");
        }
    }

}
