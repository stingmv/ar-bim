using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LoadBIMData : MonoBehaviour
{
    public List<GameObject> IfcWall;
    public List<GameObject> IfcDoor;
    public List<GameObject> IfcWindow;
    public List<GameObject> IfcSlab;
    public List<GameObject> IfcBeam;
    public List<GameObject> IfcColumn;
    public List<GameObject> IfcStair;
    public List<GameObject> IfcBuildingElementProxy;
    public List<GameObject> IfcCurtainWall;
    public List<GameObject> IfcSpace;
    public List<GameObject> IfcGridAxis;
    public List<GameObject> IfcGrid;
    public List<GameObject> IfcFlowTerminal;
    public List<GameObject> IfcRoof;
    public List<GameObject> IfcPlate;
    public List<GameObject> IfcMember;
    public List<GameObject> IfcRailing;
    public List<GameObject> IfcFooting;
    public List<GameObject> IfcCovering;
    public List<GameObject> IfcBuilding;
    public List<GameObject> IfcFlowFitting;
    public List<GameObject> IfcFlowSegment;
    public List<GameObject> IfcBuildingStorey;
    private bool isIfcWall;
    private bool isIfcDoor;
    private bool isIfcWindow;
    private bool isIfcSlab;
    private bool isIfcBeam;
    private bool isIfcColumn;
    private bool isIfcStair;
    private bool isIfcBuildingElementProxy;
    private bool isIfcCurtainWall;
    private bool isIfcSpace;
    private bool isIfcGridAxis;
    private bool isIfcGrid;
    private bool isIfcFlowTerminal;
    private bool isIfcRoof;
    private bool isIfcPlate;
    private bool isIfcMember;
    private bool isIfcRailing;
    private bool isIfcFooting;
    private bool isIfcCovering;
    private bool isIfcBuilding;
    private bool isIfcFlowFitting;
    private bool isIfcFlowSegment;
    private bool isIfcBuildingStorey;
    private string filename;

    public string Filename
    {
        get => filename;
        set => filename = value;
    }
    public bool IsIfcWall
    {
        get => isIfcWall;
        set
        {
            isIfcWall = value;
            for (int i = 0; i < IfcWall.Count; i++)
            {
                IfcWall[i].SetActive(value);
            }
        }
    }

    public bool IsIfcDoor
    {
        get => isIfcDoor;
        set
        {
            isIfcDoor = value;
            for (int i = 0; i < IfcDoor.Count; i++)
            {
                IfcDoor[i].SetActive(value);
            }
        }
    }

    public bool IsIfcWindow
    {
        get => isIfcWindow;
        set
        {
            isIfcWindow = value;
            for (int i = 0; i < IfcWindow.Count; i++)
            {
                IfcWindow[i].SetActive(value);
            }
        }
    }

    public bool IsIfcSlab
    {
        get => isIfcSlab;
        set
        {
            isIfcSlab = value;
            for (int i = 0; i < IfcSlab.Count; i++)
            {
                IfcSlab[i].SetActive(value);
            }
        }
    }

    public bool IsIfcBeam
    {
        get => isIfcBeam;
        set
        {
            isIfcBeam = value;
            for (int i = 0; i < IfcBeam.Count; i++)
            {
                IfcBeam[i].SetActive(value);
            }
        }
    }

    public bool IsIfcColumn
    {
        get => isIfcColumn;
        set
        {
            isIfcColumn = value;
            for (int i = 0; i < IfcColumn.Count; i++)
            {
                IfcColumn[i].SetActive(value);
            }
        }
    }

    public bool IsIfcStair
    {
        get => isIfcStair;
        set
        {
            isIfcStair = value;
            for (int i = 0; i < IfcStair.Count; i++)
            {
                IfcStair[i].SetActive(value);
            }
        }
    }

    public bool IsIfcBuildingElementProxy
    {
        get => isIfcBuildingElementProxy;
        set
        {
            isIfcBuildingElementProxy = value;
            for (int i = 0; i < IfcBuildingElementProxy.Count; i++)
            {
                IfcBuildingElementProxy[i].SetActive(value);
            }
        }
    }

    public bool IsIfcCurtainWall
    {
        get => isIfcCurtainWall;
        set
        {
            isIfcCurtainWall = value;
            for (int i = 0; i < IfcCurtainWall.Count; i++)
            {
                IfcCurtainWall[i].SetActive(value);
            }
        }
    }

    public bool IsIfcSpace
    {
        get => isIfcSpace;
        set
        {
            isIfcSpace = value;
            for (int i = 0; i < IfcSpace.Count; i++)
            {
                IfcSpace[i].SetActive(value);
            }
        }
    }

    public bool IsIfcGridAxis
    {
        get => isIfcGridAxis;
        set
        {
            isIfcGridAxis = value;
            for (int i = 0; i < IfcGridAxis.Count; i++)
            {
                IfcGridAxis[i].SetActive(value);
            }
        }
    }

    public bool IsIfcGrid
    {
        get => isIfcGrid;
        set
        {
            isIfcGrid = value;
            for (int i = 0; i < IfcGrid.Count; i++)
            {
                IfcGrid[i].SetActive(value);
            }
        }
    }

    public bool IsIfcFlowTerminal
    {
        get => isIfcFlowTerminal;
        set
        {
            isIfcFlowTerminal = value;
            for (int i = 0; i < IfcFlowTerminal.Count; i++)
            {
                IfcFlowTerminal[i].SetActive(value);
            }
        }
    }

    public bool IsIfcAnnotation
    {
        get => isIfcRoof;
        set
        {
            isIfcRoof = value;
            for (int i = 0; i < IfcRoof.Count; i++)
            {
                IfcRoof[i].SetActive(value);
            }
        }
    }

    public bool IsIfcPlate
    {
        get => isIfcPlate;
        set
        {
            isIfcPlate = value;
            for (int i = 0; i < IfcPlate.Count; i++)
            {
                IfcPlate[i].SetActive(value);
            }
        }
    }

    public bool IsIfcMember
    {
        get => isIfcMember;
        set
        {
            isIfcMember = value;
            for (int i = 0; i < IfcMember.Count; i++)
            {
                IfcMember[i].SetActive(value);
            }
        }
    }

    public bool IsIfcRailing
    {
        get => isIfcRailing;
        set
        {
            isIfcRailing = value;
            for (int i = 0; i < IfcRailing.Count; i++)
            {
                IfcRailing[i].SetActive(value);
            }
        }
    }

    public bool IsIfcFooting
    {
        get => isIfcFooting;
        set
        {
            isIfcFooting = value;
            for (int i = 0; i < IfcFooting.Count; i++)
            {
                IfcFooting[i].SetActive(value);
            }
        }
    }

    public bool IsIfcCovering
    {
        get => isIfcCovering;
        set
        {
            isIfcCovering = value;
            for (int i = 0; i < IfcCovering.Count; i++)
            {
                IfcCovering[i].SetActive(value);
            }
        }
    }

    public bool IsIfcBuilding
    {
        get => isIfcBuilding;
        set
        {
            isIfcBuilding = value;
            for (int i = 0; i < IfcBuilding.Count; i++)
            {
                IfcBuilding[i].SetActive(value);
            }
        }
    }

    public bool IsIfcFlowFitting
    {
        get => isIfcFlowFitting;
        set
        {
            isIfcFlowFitting = value;
            for (int i = 0; i < IfcFlowFitting.Count; i++)
            {
                IfcFlowFitting[i].SetActive(value);
            }
        }
    }

    public bool IsIfcFlowSegment
    {
        get => isIfcFlowSegment;
        set
        {
            isIfcFlowSegment = value;
            for (int i = 0; i < IfcFlowSegment.Count; i++)
            {
                IfcFlowSegment[i].SetActive(value);
            }
        }
    }
    public bool IsIfcBuildingStorey
    {
        get => isIfcBuildingStorey;
        set
        {
            isIfcBuildingStorey = value;
            for (int i = 0; i < IfcBuildingStorey.Count; i++)
            {
                IfcBuildingStorey[i].SetActive(value);
            }
        }
    }

    
}