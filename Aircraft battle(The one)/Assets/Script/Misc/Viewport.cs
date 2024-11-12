using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    float minX, maxX, minY, maxY;
    //�ӿ��м��
    float middleX;
    public float MaxX => maxX;
    void Start()
    {
        Camera mainCamera = Camera.main;
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f,mainCamera.nearClipPlane));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f,1f, mainCamera.nearClipPlane));
        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, mainCamera.nearClipPlane)).x;
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
        
        
       
        
        
    }

    //��player���ú��������ƶ�
    public Vector3 PlayerMoveablePoint(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position=Vector3.zero;
        position.x=Mathf.Clamp(playerPosition.x,minX+paddingX,maxX-paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY+paddingY, maxY-paddingY);
        return position;
    }

    //������ɵ���λ��
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 pos=Vector3.zero;
        pos.x = maxX+paddingX;
        pos.y = Random.Range(minY+paddingY,maxY-paddingY);
        return pos;
    }
    //���˵����Ұ벿���ƶ���Χ
    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(middleX + paddingX, maxX - paddingX);
        pos.y = Random.Range(minY + paddingY, maxY - paddingY);
       
        
        return pos;
    }
    //public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    //{
    //    Vector3 pos = Vector3.zero;
    //    pos.x = Random.Range(minX + paddingX, maxX - paddingX);
    //    pos.y = Random.Range(minY + paddingY, maxY - paddingY);
    //    return pos;
    //}
}
