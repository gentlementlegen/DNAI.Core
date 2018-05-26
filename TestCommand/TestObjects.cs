using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CoreControl;
using static CoreControl.EntityFactory;
using CoreCommand.Command;
using CoreCommand.Command.Declarator;
using CoreCommand.Command.Class;
using CoreCommand.Command.Function;
using CoreCommand.Command.Function.Instruction;

namespace TestCommand
{
    /// <summary>
    /// Description résumée pour TestObjects
    /// </summary>
    [TestClass]
    public class TestObjects
    {
        public TestObjects()
        {
            //
            // TODO: ajoutez ici la logique du constructeur
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active, ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Attributs de tests supplémentaires
        //
        // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
        //
        // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private uint GenerateObjectInto(CoreCommand.BinaryManager manager, string name, Dictionary<string, uint> attributes)
        {
            //Declare the object entity
            Declare.Reply objDeclared;
            Assert.IsTrue(manager.CallCommand(new Declare
            {
                ContainerID = (uint)BASE_ID.GLOBAL_CTX,
                EntityType = ENTITY.OBJECT_TYPE,
                Name = name,
                Visibility = VISIBILITY.PUBLIC
            }, out objDeclared));

            if (attributes.Count == 0)
                return objDeclared.EntityID;


            //declare each attribute
            foreach (KeyValuePair<string, uint> curr in attributes)
            {
                Assert.IsTrue(manager.CallCommand(new AddAttribute
                {
                    ClassId = objDeclared.EntityID,
                    Name = curr.Key,
                    TypeId = curr.Value,
                    Visibility = VISIBILITY.PUBLIC
                }));
            }

            //Declare the set function
            Declare.Reply setFunction;
            Assert.IsTrue(manager.CallCommand(new Declare
            {
                ContainerID = objDeclared.EntityID,
                EntityType = ENTITY.FUNCTION,
                Name = "Set",
                Visibility = VISIBILITY.PUBLIC
            }, out setFunction));

            //Set as member
            SetFunctionAsMember.Reply thisParam;
            Assert.IsTrue(manager.CallCommand(new SetFunctionAsMember
            {
                ClassId = objDeclared.EntityID,
                Name = "Set",
                Visibility = VISIBILITY.PUBLIC
            }, out thisParam));
            
            //Declare function parameter
            Declare.Reply tocopyParameter;
            Assert.IsTrue(manager.CallCommand(new Declare
            {
                ContainerID = setFunction.EntityID,
                EntityType = ENTITY.VARIABLE,
                Name = "tocopy",
                Visibility = VISIBILITY.PUBLIC
            }, out tocopyParameter));
            Assert.IsTrue(manager.CallCommand(new CoreCommand.Command.Variable.SetType
            {
                VariableID = tocopyParameter.EntityID,
                TypeID = objDeclared.EntityID
            }));
            Assert.IsTrue(manager.CallCommand(new SetParameter
            {
                FuncId = setFunction.EntityID,
                ExternalVarName = "tocopy"
            }));
            
            //get tocopy parameter
            AddInstruction.Reply gettocopy;
            Assert.IsTrue(manager.CallCommand(new AddInstruction
            {
                FunctionID = setFunction.EntityID,
                ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                Arguments = new List<uint> { tocopyParameter.EntityID }
            }, out gettocopy));

            //get toCopy parameter attributes
            AddInstruction.Reply getattr;
            Assert.IsTrue(manager.CallCommand(new AddInstruction
            {
                FunctionID = setFunction.EntityID,
                ToCreate = InstructionFactory.INSTRUCTION_ID.GET_ATTRIBUTES,
                Arguments = new List<uint> { objDeclared.EntityID }
            }, out getattr));
            Assert.IsTrue(manager.CallCommand(new LinkData
            {
                FunctionID = setFunction.EntityID,
                FromId = gettocopy.InstructionID,
                OutputName = "reference",
                ToId = getattr.InstructionID,
                InputName = "this"
            }));

            //set this parameter attributes
            AddInstruction.Reply setattr;
            Assert.IsTrue(manager.CallCommand(new AddInstruction
            {
                FunctionID = setFunction.EntityID,
                ToCreate = InstructionFactory.INSTRUCTION_ID.SET_ATTRIBUTES,
                Arguments = new List<uint> { thisParam.ThisParamID }
            }, out setattr));

            //link each attributes
            foreach (string curr in attributes.Keys)
            {
                Assert.IsTrue(manager.CallCommand(new LinkData
                {
                    FunctionID = setFunction.EntityID,
                    FromId = getattr.InstructionID,
                    OutputName = curr,
                    ToId = setattr.InstructionID,
                    InputName = curr
                }));
            }

            //set the function entry point
            Assert.IsTrue(manager.CallCommand(new SetEntryPoint
            {
                FunctionId = setFunction.EntityID,
                Instruction = setattr.InstructionID
            }));

            return objDeclared.EntityID;
        }

        [TestMethod]
        public void GenerateObjectVector()
        {
            CoreCommand.BinaryManager manager = new CoreCommand.BinaryManager();

            GenerateObjectInto(manager, "Vector", new Dictionary<string, uint>
            {
                { "x", (uint)BASE_ID.INTEGER_TYPE },
                { "y", (uint)BASE_ID.INTEGER_TYPE },
                { "z", (uint)BASE_ID.INTEGER_TYPE }
            });
            manager.SaveCommandsTo("vector.dnai");
        }

        [TestMethod]
        public void GenerateObjectVertex()
        {
            CoreCommand.BinaryManager manager = new CoreCommand.BinaryManager();

            uint vector3Df = GenerateObjectInto(manager, "Vector3Df", new Dictionary<string, uint>
            {
                { "x", (uint)BASE_ID.FLOATING_TYPE },
                { "y", (uint)BASE_ID.FLOATING_TYPE },
                { "z", (uint)BASE_ID.FLOATING_TYPE }
            });
            uint vector2Di = GenerateObjectInto(manager, "Vector2Di", new Dictionary<string, uint>
            {
                { "x", (uint)BASE_ID.INTEGER_TYPE },
                { "y", (uint)BASE_ID.INTEGER_TYPE }
            });
            uint color = GenerateObjectInto(manager, "Color", new Dictionary<string, uint>
            {
                { "r", (uint)BASE_ID.INTEGER_TYPE },
                { "g", (uint)BASE_ID.INTEGER_TYPE },
                { "b", (uint)BASE_ID.INTEGER_TYPE },
                { "a", (uint)BASE_ID.INTEGER_TYPE }
            });
            GenerateObjectInto(manager, "Vertex", new Dictionary<string, uint>
            {
                { "position", vector3Df },
                { "normal", vector3Df },
                { "texturecoord", vector2Di },
                { "color", color }
            });
            manager.SaveCommandsTo("vertex.dnai");
        }

        [TestMethod]
        public void GeneratePositionGraph()
        {
            CoreCommand.BinaryManager manager = new CoreCommand.BinaryManager();

            uint Position = GenerateObjectInto(manager, "Position", new Dictionary<string, uint>
            {
                { "x", (uint)BASE_ID.FLOATING_TYPE },
                { "y", (uint)BASE_ID.FLOATING_TYPE },
                { "z", (uint)BASE_ID.FLOATING_TYPE }
            });

            Declare.Reply posList;
            Assert.IsTrue(manager.CallCommand(new Declare
            {
                ContainerID = (uint)BASE_ID.GLOBAL_CTX,
                EntityType = ENTITY.LIST_TYPE,
                Name = "PositionList",
                Visibility = VISIBILITY.PUBLIC
            }, out posList));
            Assert.IsTrue(manager.CallCommand(new CoreCommand.Command.List.SetType
            {
                ListId = posList.EntityID,
                TypeId = Position
            }));

            Declare.Reply indexList;
            Assert.IsTrue(manager.CallCommand(new Declare
            {
                ContainerID = (uint)BASE_ID.GLOBAL_CTX,
                EntityType = ENTITY.LIST_TYPE,
                Name = "IndexList",
                Visibility = VISIBILITY.PUBLIC
            }, out indexList));
            Assert.IsTrue(manager.CallCommand(new CoreCommand.Command.List.SetType
            {
                ListId = indexList.EntityID,
                TypeId = (uint)BASE_ID.INTEGER_TYPE
            }));

            Declare.Reply adjList;
            Assert.IsTrue(manager.CallCommand(new Declare
            {
                ContainerID = (uint)BASE_ID.GLOBAL_CTX,
                EntityType = ENTITY.LIST_TYPE,
                Name = "AdjacentList",
                Visibility = VISIBILITY.PUBLIC
            }, out adjList));
            Assert.IsTrue(manager.CallCommand(new CoreCommand.Command.List.SetType
            {
                ListId = adjList.EntityID,
                TypeId = indexList.EntityID
            }));

            GenerateObjectInto(manager, "PositionGraph", new Dictionary<string, uint>
            {
                { "positions", posList.EntityID },
                { "links", adjList.EntityID }
            });

            manager.SaveCommandsTo("position_graph.dnai");
        }
    }
}
