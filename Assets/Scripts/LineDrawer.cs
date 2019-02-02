using data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class LineDrawer
    {
        public enum NodeType
        {
            CurrentlyStanding,
            InDangerZone,
            AllowedToMove,
            TooFar,
            End,
            Start
        }

        private struct nodeLineVisuals
        {
            public float width;
            public Color color;
            public string sortingLayer;
            public float radius;
        }

        private readonly Dictionary<NodeType, nodeLineVisuals> nodeVisuals = new Dictionary<NodeType, nodeLineVisuals>()
        {
            {
                NodeType.CurrentlyStanding,
                new nodeLineVisuals
                {
                    width =  0.1f,
                    color = Color.white,
                    sortingLayer = "Lines",
                    radius = 0.2f
                }
            },
            {
                NodeType.InDangerZone,
                new nodeLineVisuals
                {
                    width =  0.04f,
                    color = new Color(0.5f, 0, 0.3f),
                    sortingLayer = "Lines",
                    radius = 0.1f
                }
            },
            {
                NodeType.TooFar,
                new nodeLineVisuals
                {
                    width =  0.045f,
                    color = Color.gray,
                    sortingLayer = "Lines",
                    radius = 0.115f
                }
            },
            {
                NodeType.AllowedToMove,
                new nodeLineVisuals
                {
                    width =  0.06f,
                    color = Color.red,
                    sortingLayer = "Lines",
                    radius = 0.2f
                }
            },
        };

        private readonly Material lineMaterial;
        private readonly int segments = 25;

        public LineDrawer(Material lineMaterial)
        {
            this.lineMaterial = lineMaterial;
        }

        /// <summary>
        /// Draws a circle around node. If not looted, creates dot in the middle
        /// </summary>
        public List<GameObject> drawNodeCircle(GameObject go, NodeType type, bool alreadyLooted, Material lineMaterial)
        {
            var lineObjects = new List<GameObject>();

            // Initialize outer circle
            var outerCircleGO = new GameObject();
            outerCircleGO.transform.position = go.transform.position;
            lineObjects.Add(outerCircleGO);
            var linerendererOuterCircle = outerCircleGO.AddComponent<LineRenderer>();
            var visuals = nodeVisuals[type];
            var radius = visuals.radius;
            SetNodeVisuals(linerendererOuterCircle, visuals, lineMaterial);

            // Initialize inner circle if not looted
            var innerCircleGO = new GameObject();
            innerCircleGO.transform.position = go.transform.position;
            lineObjects.Add(innerCircleGO);
            var linerendererInnerCircle = innerCircleGO.AddComponent<LineRenderer>();
            if (!alreadyLooted)
            {
                SetNodeVisuals(linerendererInnerCircle, nodeVisuals[type], lineMaterial);
            }

            float x, y;
            float angle = 1f;

            for (int i = 0; i < (segments + 1); i++)
            {
                // Set one circle segment pos for outer circle
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                linerendererOuterCircle.SetPosition(i, new Vector3(x, y, 0));

                if (!alreadyLooted)
                {
                    // Set one circle segment pos for inner circle
                    x = Mathf.Sin(Mathf.Deg2Rad * angle) * 0.04f;
                    y = Mathf.Cos(Mathf.Deg2Rad * angle) * 0.04f;
                    linerendererInnerCircle.SetPosition(i, new Vector3(x, y, 0));
                }


                // Little over full circle so no gaps
                angle += (380f / segments);
            }

            return lineObjects;
        }

        private void SetNodeVisuals(LineRenderer lr, nodeLineVisuals vis, Material lineMaterial)
        {
            lr.widthMultiplier = vis.width;
            lr.startColor = lr.endColor = vis.color;
            lr.sortingLayerName = vis.sortingLayer;
            lr.material = lineMaterial;
            lr.positionCount = segments + 1;
            lr.useWorldSpace = false;
        }

        public GameObject DrawLine(GameObject fromObject, Vector3 toPos, GameObject parent, bool allowedToMoveTo)
        {
            var radius = 0.06f;

            var s = new GameObject();
            s.transform.position = fromObject.transform.position;
            s.transform.SetParent(parent.transform);
            var linerenderer = s.AddComponent<LineRenderer>();
            linerenderer.startColor = Color.white;
            linerenderer.endColor = Color.white;

            if (allowedToMoveTo)
            {
                linerenderer.widthMultiplier = 0.085f;
                linerenderer.material = lineMaterial;
            }
            else
            {
                linerenderer.widthMultiplier = 0.06f;
                linerenderer.material = lineMaterial;
            }

            var radiusStartPos = fromObject.transform.position - (fromObject.transform.position - toPos).normalized * (1 + radius / (Vector3.Distance(fromObject.transform.position, toPos))) / 2.3f;
            var radiusEndPos = toPos - (toPos - fromObject.transform.position).normalized * (1 + radius / (Vector3.Distance(fromObject.transform.position, toPos))) / 2.3f;
            linerenderer.SetPosition(0, radiusStartPos);
            linerenderer.SetPosition(1, radiusEndPos);
            linerenderer.sortingLayerName = "Lines";
            return s;
        }
    }
}
