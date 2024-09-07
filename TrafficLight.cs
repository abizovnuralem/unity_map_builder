﻿#region License
/******************************************************************************
* Copyright 2018-2020 The AutoCore Authors. All Rights Reserved.
* 
* Licensed under the GNU Lesser General Public License, Version 3.0 (the "License"); 
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
* 
* https://www.gnu.org/licenses/lgpl-3.0.html
* 
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*****************************************************************************/
#endregion

using UnityEditor;
using UnityEngine;

namespace Packages.MapToolbox
{
    class TrafficLight : WayTypeBase<TrafficLight>
    {
        public float height = 2;
        protected override void Start()
        {
            base.Start();
            LineRenderer.startWidth = LineRenderer.endWidth = 0.1f;
            LineRenderer.startColor = LineRenderer.endColor = Color.green;
        }
        internal void OnEditorEnable()
        {
            SceneView.duringSceneGui -= DuringSceneGui;
            SceneView.duringSceneGui += DuringSceneGui;
        }
        internal void OnEditorDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGui;
            if (gameObject != null)
            {
                SceneVisibilityManager.instance.EnablePicking(gameObject, true);
            }
        }
        private void DuringSceneGui(SceneView obj)
        {
            if (EditorUpdate.MouseInSceneView)
            {
                if (EditorUpdate.MouseLeftButtonDownWithCtrl && EditorUpdate.MouseLeftButtonDownWithShift)
                {
                    RemovePoints();
                    UpdateRenderer();
                    SceneVisibilityManager.instance.DisablePicking(gameObject, true);
                }
                else if (EditorUpdate.MouseLeftButtonDownWithCtrl)
                {
                    AddPoints();
                    UpdateRenderer();
                    SceneVisibilityManager.instance.DisablePicking(gameObject, true);
                }
            }
        }

        private void AddPoints()
        {
            var point = Utils.MousePointInSceneView;
            point.y = 5;
            if (Way.Nodes.Count < 2)
            {
                Way.InsertNode(point);
            }
        }

        private void RemovePoints()
        {
            Way.RemoveNode(Way.Nodes.Count - 1);
        }
    }
    [CustomEditor(typeof(TrafficLight))]
    class TrafficLightEditor : Editor
    {
        TrafficLight Target => target as TrafficLight;
        private void OnEnable() => Target.OnEditorEnable();
        private void OnDisable() => Target.OnEditorDisable();
    }
}