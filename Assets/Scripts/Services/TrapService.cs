using UnityEngine;
using System.IO;


namespace BeastHunter
{
    public class TrapService : Service
    {
        #region Contants

        private const string TRAPS_SAVE_FILE_PATH = "Assets/Resources/traps";

        #endregion


        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public TrapService(Contexts contexts) : base(contexts)
        {
            _context = contexts as GameContext;
        }

        #endregion


        #region Methods

        public void SaveTraps()
        {
            if(_context.TrapModels.Count > 0)
            {
                string trapsDataString = "";

                foreach (var trap in _context.TrapModels)
                {
                    trapsDataString += JsonUtility.ToJson(trap.Value.TrapData.TrapType) + "\n";
                    trapsDataString += JsonUtility.ToJson(trap.Value.Trap.transform.position) + "\n";
                    trapsDataString += JsonUtility.ToJson(trap.Value.Trap.transform.eulerAngles) + "\n";
                }

                File.WriteAllText(TRAPS_SAVE_FILE_PATH, trapsDataString);
            }
        }

        public void LoadTraps()
        {
            if (File.Exists(TRAPS_SAVE_FILE_PATH))
            {
                string[] lines = File.ReadAllLines(TRAPS_SAVE_FILE_PATH);

                for (int line = 0; line < lines.Length; line += 3)
                {
                    TrapsEnum type = JsonUtility.FromJson<TrapsEnum>(lines[line]);
                    Vector3 position = JsonUtility.FromJson<Vector3>(lines[line + 1]);
                    Vector3 eulers = JsonUtility.FromJson<Vector3>(lines[line + 2]);

                    switch (type)
                    {
                        case TrapsEnum.None:
                            break;
                        case TrapsEnum.BearTrap:
                            new InitializeTrapController(_context, Data.TrapData, position, eulers, true);
                            break;
                        case TrapsEnum.AcidCatapult:
                            new InitializeTrapController(_context, Data.TrapData2, position, eulers, true);
                            break;
                        default:
                            break;
                    }
                }
            }          
        }

        #endregion
    }
}

