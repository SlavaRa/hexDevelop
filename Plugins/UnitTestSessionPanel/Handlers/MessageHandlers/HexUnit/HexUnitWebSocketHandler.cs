﻿using System;
using System.Collections.Generic;
using ASCompletion.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Windows.Forms;

namespace UnitTestSessionsPanel.Handlers.MessageHandlers.HexUnit
{
    class HexUnitWebSocketHandler
    {
        private WebSocketServer server;

        public HexUnitWebSocketHandler(TestsSessionsPanel pluginUi)
        {
            server = new WebSocketServer(6660);
            server.ReuseAddress = true;
            server.AddWebSocketService("/", () => new HexUnitWebSocketBehavior(pluginUi));
            server.AddWebSocketService("/hexunit", () => new HexUnitWebSocketBehavior(pluginUi));
            server.Start();
        }

        private class HexUnitWebSocketBehavior : WebSocketBehavior
        {
            private TestsSessionsPanel pluginUi;

            private HexUnitHelper helper = new HexUnitHelper();

            public HexUnitWebSocketBehavior()
                : this(null)
            {

            }

            public HexUnitWebSocketBehavior(TestsSessionsPanel pluginUi)
            {
                this.pluginUi = pluginUi;
            }

            protected override void OnOpen()
            {
                //Log("Connection incoming");
            }

            protected override void OnError(ErrorEventArgs e)
            {
                //Log("Error " + e.Message);
            }

            protected override void OnMessage(MessageEventArgs e)
            {
                var info = helper.ParseMessage(e.Data);
                if (info != null)
                {
                    if (pluginUi.InvokeRequired)
                    {
                        pluginUi.Invoke((MethodInvoker)delegate
                        {
                            pluginUi.AddTest(info);
                            pluginUi.EndUpdate();
                        });
                    }
                    else
                    {
                        pluginUi.AddTest(info);
                        pluginUi.EndUpdate();
                    }
                    
                }
            }
        }
    }
}
