using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StatOMat Negativ", menuName = "StatOMat/Data/Negativ")]
public class StatOMatNegativData : ScriptableObject
{
    public void RandomizeStats()
    {
        resultMaxHP = Random.Range(min_MaxHp, max_MaxHp);
        resultAttackDMG = Random.Range(min_MeleeAttackDMG, max_MeleeAttackDMG);
        resultRangedAttackDMG = Random.Range(min_RangedAttackDMG, max_RangedAttackDMG);
        resultMoveSPD = Random.Range(min_MoveSpeed, max_MoveSpeed);
        resultMeleeAttackSPD = Random.Range(min_MeleeAttackSPD, max_MeleeAttackSPD);
        resultRangedAttackSPD = Random.Range(min_RangedAttackSPD, max_RangedAttackSPD);
        resultMeleeDEF = Random.Range(min_MeleeDEF, max_MeleeDEF);
        resultRangedDEF = Random.Range(min_RangedDEF, max_RangedDEF);
        resultBulletSPD = Random.Range(min_BulletSPD, max_BulletSPD);
    }

    [Header("*** Ranges for the Randomizer ***")]
    #region StatRanges
    [Header("Range: MAXHP")]
    [SerializeField]
    public int min_MaxHp;
    [SerializeField]
    public int max_MaxHp;

    [HideInInspector]
    public int resultMaxHP;

    [Header("Range: MeleeAtkDMG")]
    [SerializeField]
    public int min_MeleeAttackDMG;
    [SerializeField]
    public int max_MeleeAttackDMG;

    [HideInInspector]
    public int resultAttackDMG;

    [Header("Range: RangedAtkDMG")]
    [SerializeField]
    public int min_RangedAttackDMG;
    [SerializeField]
    public int max_RangedAttackDMG;

    [HideInInspector]
    public int resultRangedAttackDMG;

    [Header("Range: MeleeDef")]
    [SerializeField]
    public int min_MeleeDEF;
    [SerializeField]
    public int max_MeleeDEF;

    [HideInInspector]
    public int resultMeleeDEF;

    [Header("Range: RangedDef")]
    [SerializeField]
    public int min_RangedDEF;
    [SerializeField]
    public int max_RangedDEF;

    [HideInInspector]
    public int resultRangedDEF;

    [Header("Range: MoveSpeed")]
    [SerializeField]
    public float min_MoveSpeed;
    [SerializeField]
    public float max_MoveSpeed;

    [HideInInspector]
    public float resultMoveSPD;

    [Header("Range: MeleeAtkSPD")]
    [SerializeField]
    public int min_MeleeAttackSPD;
    [SerializeField]
    public int max_MeleeAttackSPD;

    [HideInInspector]
    public int resultMeleeAttackSPD;

    [Header("Range: RangedAtkSPD")]
    [SerializeField]
    public int min_RangedAttackSPD;
    [SerializeField]
    public int max_RangedAttackSPD;

    [HideInInspector]
    public int resultRangedAttackSPD;

    [Header("Range: BulletSPD")]
    [SerializeField]
    public float min_BulletSPD;
    [SerializeField]
    public float max_BulletSPD;

    [HideInInspector]
    public float resultBulletSPD;

    #endregion
}


