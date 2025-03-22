using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    public void Transition(string from,string to,Transform playerTransform, Vector3 targetPosition)
    {
        StartCoroutine(TransitionToScene(from, to,playerTransform,targetPosition));
        playerTransform.position = targetPosition;
    }

    private IEnumerator TransitionToScene(string from, string to, Transform playerTransform, Vector3 targetPosition)
    {
        yield return SceneManager.UnloadSceneAsync(from);
        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneByName(to);
        SceneManager.SetActiveScene(newScene);

        //“∆∂Ø»ÀŒÔ
        //playerTransform.position = targetPosition.position;
        //playerTransform.rotation = targetPosition.rotation;

    }
}
