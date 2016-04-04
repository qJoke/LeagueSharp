namespace DZLib.Modules
{
    interface IModule
    {
        void OnLoad();

        bool ShouldGetExecuted();

        ModuleType GetModuleType();

        void OnExecute();
    }
}
