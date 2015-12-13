namespace SoloVayne.Modules
{
    interface ISOLOModule
    {
        void OnLoad();

        bool ShouldGetExecuted();

        ModuleType GetModuleType();

        void OnExecute();
    }
}
