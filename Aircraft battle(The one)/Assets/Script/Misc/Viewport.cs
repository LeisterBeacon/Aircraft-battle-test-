using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    float minX, maxX, minY, maxY;
    //视口中间点
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

    //让player调用函数限制移动
    public Vector3 PlayerMoveablePoint(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position=Vector3.zero;
        position.x=Mathf.Clamp(playerPosition.x,minX+paddingX,maxX-paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY+paddingY, maxY-paddingY);
        return position;
    }

    //随机生成敌人位置
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 pos=Vector3.zero;
        pos.x = maxX+paddingX;
        pos.y = Random.Range(minY+paddingY,maxY-paddingY);
        return pos;
    }
    //敌人的在右半部分移动范围
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
