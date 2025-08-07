using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tool
{
    public string name;
    public string objectTag;
    public string lampTag;

    public float cooldownTime;
    public float deployDistance;
    public float moveDuration;

    private MonoBehaviour runner;
    private GameObject toolObject;
    private Image lampImage;

    public bool IsDeployed { get; private set; } = false;
    public bool IsBusy { get; private set; } = false;
    public float Cooldown { get; private set; } = 0f;

    public Tool(MonoBehaviour runner, string name, string objectTag, string lampTag, float deployDistance, float moveDuration, float cooldownTime)
    {
        this.runner = runner;
        this.name = name;
        this.objectTag = objectTag;
        this.lampTag = lampTag;
        this.deployDistance = deployDistance;
        this.moveDuration = moveDuration;
        this.cooldownTime = cooldownTime;

        toolObject = GameObject.FindGameObjectWithTag(objectTag);
        lampImage = GameObject.FindGameObjectWithTag(lampTag).GetComponent<Image>();
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (Cooldown > 0f)
            Cooldown -= deltaTime;
    }

    public void Toggle()
    {
        if (IsBusy)
            return;

        IsDeployed = !IsDeployed;
        runner.StartCoroutine(DeployRoutine(IsDeployed));
    }

    private IEnumerator DeployRoutine(bool deploy)
    {
        IsBusy = true;
        lampImage.color = Color.yellow;

        Vector3 startPos = toolObject.transform.localPosition;
        Vector3 endPos = startPos + (deploy ? Vector3.up : Vector3.down) * deployDistance;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            toolObject.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        toolObject.transform.localPosition = endPos;
        lampImage.color = deploy ? Color.green : Color.red;
        IsBusy = false;
    }

    public bool IsReady()
    {
        return IsDeployed && !IsBusy && Cooldown <= 0f;
    }

    public void TriggerCooldown()
    {
        Cooldown = cooldownTime;
    }
}
