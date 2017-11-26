using System.IO;

namespace CoreCommand
{
    public enum COMMANDS
    {
        DECLARE
    }

    public interface IManager
    {
        /// <summary>
        /// Serialize saved commands into a file
        /// </summary>
        /// <param name="filename">Name of the file in which serialize commands</param>
        void SaveCommandsTo(string filename);

        /// <summary>
        /// Deserialize saved commands from a file
        /// </summary>
        /// <param name="filename">Name of the file from which load commands</param>
        void LoadCommandsFrom(string filename);

        /// <summary>
        /// Method to handle a CoreControl.Controller.Declare call
        /// </summary>
        /// <param name="inStream">Input stream from which read the command</param>
        /// <param name="outStream">Output stream on which write the reply</param>
        void OnDeclare(Stream inStream, Stream outStream);

        void OnRemove(Stream inStream, Stream outStream);

        void OnMove(Stream inStream, Stream outStream);

        void OnChangeVisibility(Stream inStream, Stream outStream);

        /// <summary>
        /// Method to handle CoreControl.Controller.SetVariableValue call
        /// </summary>
        /// <param name="inStream">Input stream from which read the command</param>
        /// <param name="outStream">Output stream on which write the reply</param>
        void OnSetVariableValue(Stream inStream, Stream outStream);

        /// <summary>
        /// Method to handle CoreControl.Controller.SetVariableType call
        /// </summary>
        /// <param name="inStream">Input stream from which read the command</param>
        /// <param name="outStream">Output stream on which write the reply</param>
        void OnSetVariableType(Stream inStream, Stream outStream);

        void OnGetVariableValue(Stream inStream, Stream outStream);

        void OnSetContextParent(Stream inStream, Stream outStream);

        void OnSetEnumerationType(Stream inStream, Stream outStream);

        void OnSetEnumerationValue(Stream inStream, Stream outStream);

        void OnGetEnumerationValue(Stream inStream, Stream outStream);

        void OnRemoveEnumerationValue(Stream inStream, Stream outStream);

        void OnAddClassAttribute(Stream inStream, Stream outStream);

        void OnRenameClassAttribute(Stream inStream, Stream outStream);

        void OnRemoveClassAttribute(Stream inStream, Stream outStream);

        void OnAddClassMemberFunction(Stream inStream, Stream outStream);

        void OnSetListType(Stream inStream, Stream outStream);

        void OnCallFunction(Stream inStream, Stream outStream);

        void OnSetFunctionParameter(Stream inStream, Stream outStream);

        void OnSetFunctionReturn(Stream inStream, Stream outStream);

        void OnSetFunctionEntryPoint(Stream inStream, Stream outStream);

        void OnRemoveFunctionInstruction(Stream inStream, Stream outStream);

        void OnAddInstruction(Stream inStream, Stream outStream);

        void OnLinkInstructionExecution(Stream inStream, Stream outStream);

        void OnLinkInstructionData(Stream inStream, Stream outStream);

        void OnSetInstructionInputValue(Stream inStream, Stream outStream);

        void OnUnlinkInstructionFlow(Stream inStream, Stream outStream);

        void OnUnlinkInstructionInput(Stream inStream, Stream outStream);
    }
}