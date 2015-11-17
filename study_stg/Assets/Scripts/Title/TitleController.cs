using UnityEngine;
using System.Collections;

/// <summary>
///   タイトル画面の制御
/// </summary>
public class TitleController : MonoBehaviour {
	void Update () {
        if (Input.GetKey(KeyCode.Z))
        {
            Application.LoadLevel("MainGame");
        }
	}
}
