using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

namespace NotTetrin.Ingame {
    public class Ranking : MonoBehaviour {
        private static readonly int FetchCount = 10;

        [SerializeField]
        private Text textField;

        private List<Ranker> rankers = new List<Ranker>();
        
        public void Fetch() {
            try {
                var query = new NCMBQuery<NCMBObject>(@"Ranking");
                query.OrderByDescending("score");
                query.Limit = FetchCount;
                query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                    if (e != null) {
                        Debug.LogError(e.Message);
                    } else {
                        var list = new List<Ranker>();
                        foreach (NCMBObject obj in objList) {
                            string name = System.Convert.ToString(obj["name"]);
                            int score = System.Convert.ToInt32(obj["score"]);
                            list.Add(new Ranker(name, score));
                        }
                        rankers = list;
                    }

                    var text = "";
                    for (int i = 0; i < rankers.Count; i++) {
                        text += rankers[i].ToString();
                        if (i < rankers.Count - 1) {
                            text += Environment.NewLine;
                        }
                    }
                    textField.text = text;
                });
            } catch (Exception e) {
                Debug.LogError(e.Message);
                textField.text = @"ランキングの取得に失敗";
            }
        }

        public bool Save(Ranker ranker) {
            try {
                var ncmbObj = new NCMBObject(@"Ranking");
                ncmbObj[@"name"] = ranker.Name;
                ncmbObj[@"score"] = ranker.Score;

                if (PlayerPrefs.HasKey(@"object_id")) {
                    ncmbObj.ObjectId = PlayerPrefs.GetString(@"object_id");
                    ncmbObj.SaveAsync();
                } else {
                    ncmbObj.SaveAsync((NCMBException e) => PlayerPrefs.SetString(@"object_id", ncmbObj.ObjectId));
                }
            } catch (Exception e) {
                Debug.LogError(e.Message);
                return false;
            }
            return true;
        }
    }
}