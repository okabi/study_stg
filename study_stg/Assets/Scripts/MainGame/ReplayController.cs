using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using StudySTG;

/// <summary>
///   リプレイの制御を扱うクラス
/// </summary>
public class ReplayController : MonoBehaviour
{
    /// <summary>アタッチされているReplayStatus</summary>
    private ReplayStatus replayStatus;


    void Awake()
    {
        replayStatus = GetComponent<ReplayStatus>();
        replayStatus.playerInput = new List<byte>();
        replayStatus.inputIndex = 0;
        if (GameObject.Find("SaveController") != null && GameObject.Find("SaveController").GetComponent<SaveStatus>().replaying)
        {
            Load();
        }
    }


    /// <summary>
    ///   replay.rpy を読み込む．読み込めなかったら false を返す．
    /// </summary>
    /// <returns>読み込めたらtrue，そうじゃなかったらfalse</returns>
    public bool Load()
    {
        bool retval = false;
        string fileName = @"replay.rpy";
        if (File.Exists(fileName))
        {
            try
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead(fileName)))
                {
                    replayStatus.dateTime = br.ReadInt64();
                    replayStatus.seed = br.ReadInt32();
                    replayStatus.rank = br.ReadInt32();
                    int dataNum = br.ReadInt32();
                    for (int i = 0; i < dataNum; i++)
                    {
                        replayStatus.playerInput.Add(br.ReadByte());
                    }
                }
                retval = true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        return retval;
    }


    /// <summary>
    ///   replay(datetime) の形式で現在のリプレイを保存し，プレイログも出力する．
    /// </summary>
    /// <param name="playerStatus">プレイヤー情報</param>
    /// <param name="onlyPlaydata">プレイデータログのみ出力する</param>
    public void Save(PlayerStatus playerStatus, bool onlyPlaydata)
    {
        DateTime dt = DateTime.FromBinary(replayStatus.dateTime);
        string fileName = String.Format(
            "replay{0}-{1:D2}-{2:D2}-{3:D2}{4:D2}{5:D2}.rpy",
            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        try
        {
            if (!onlyPlaydata)
            {
                using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileName)))
                {
                    bw.Write(replayStatus.dateTime);
                    bw.Write(replayStatus.seed);
                    bw.Write(replayStatus.rank);
                    bw.Write(replayStatus.playerInput.Count);
                    foreach (byte input in replayStatus.playerInput)
                    {
                        bw.Write(input);
                    }
                }
            }
            fileName = String.Format(
                "playlog{0}-{1:D2}-{2:D2}-{3:D2}{4:D2}{5:D2}.dat",
                dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            using (var file = new StreamWriter(fileName))
            {
                file.WriteLine(String.Format("TimeStamp,Start,{0}", dt.ToString()));
                file.WriteLine(String.Format("Score,Total,{0}", playerStatus.score));
                foreach (Define.EnemyTag tag in System.Enum.GetValues(typeof(Define.EnemyTag)))
                {
                    file.WriteLine(String.Format("Score,{0},{1}", tag.ToString(), playerStatus.tagScore[tag]));
                }
                for (int i = 0; i < playerStatus.missFactor.Count; i++)
                {
                    file.WriteLine(String.Format("Miss Factor,{0},{1}", i + 1, playerStatus.missFactor[i].ToString()));
                }
                for (int i = 0; i < playerStatus.missTime.Count; i++)
                {
                    file.WriteLine(String.Format("Miss Time,{0},{1}", i + 1, playerStatus.missTime[i].ToString()));
                }
                foreach (Define.EnemyTag tag in System.Enum.GetValues(typeof(Define.EnemyTag)))
                {
                    for (int i = 0; i < playerStatus.minDistance[tag].Count; i++)
                    {
                        file.WriteLine(String.Format("Min Distance,{0}-{1},{2}", tag.ToString(), i + 1, playerStatus.minDistance[tag][i].ToString()));
                    }
                }
                foreach (Define.EnemyTag tag in System.Enum.GetValues(typeof(Define.EnemyTag)))
                {
                    for (int i = 0; i < playerStatus.lockoned[tag].Count; i++)
                    {
                        file.WriteLine(String.Format("Lockon,{0}-{1},{2}", tag.ToString(), i + 1, playerStatus.lockoned[tag][i].ToString()));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }


    /// <summary>
    ///   プレイヤー入力を1フレーム分読み込む
    /// </summary>
    /// <returns>プレイヤー入力のDictionary</returns>
    public Dictionary<PlayerStatus.CommandType, bool> LoadInput()
    {
        var retval = new Dictionary<PlayerStatus.CommandType, bool>();
        foreach (PlayerStatus.CommandType type in System.Enum.GetValues(typeof(PlayerStatus.CommandType)))
        {
            retval[type] = ((int)replayStatus.playerInput[replayStatus.inputIndex] & (1 << (int)type)) > 0; 
        }
        replayStatus.inputIndex += 1;
        return retval;
    }


    /// <summary>
    ///   プレイヤー入力を1フレーム分保存する
    /// </summary>
    /// <param name="command">入力キーと押し続けたフレームのDictionary</param>
    public void SaveInput(Dictionary<PlayerStatus.CommandType, int> command)
    {
        int input = 0;
        foreach (var c in command)
        {
            input += c.Value > 0 ? 1 << (int)c.Key : 0;
        }
        replayStatus.playerInput.Add((byte)input);
    }
}
