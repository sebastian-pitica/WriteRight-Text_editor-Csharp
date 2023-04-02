using CommonsModule;
using System;
using System.Windows.Forms;

/**************************************************************************
 *                                                                        *
 *  File:        Interfaces.cs                                            *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține interfețele, sub formele de Interface   *
 *  sau Abstract Class, utilizate în cadrul proiectului WriteRight.       *
 *  Scopul lor este de a pune în aplicare polimorfismul pentru clasele    *
 *  concrete, și de a oferi o structură bine definită acestora.           *
 *                                                                        *
 **************************************************************************/


namespace Interfaces
{
    /// <summary>
    /// Interfața pentru toate obiectele ce vor implementa funcționalitățile GUI. 
    /// Utilizată pentru implementarea Design Patternului Singleton împreună cu Command.
    /// </summary>
    public abstract class ISingletonCommand
    {
        protected ISingletonCommand() { }
        abstract public void Execute();
        static public ISingletonCommand GetCommandObj() { throw new NotImplementedException(); }
    }

    /// <summary>
    /// Interfața pentru toate obiectele de tip Singleton-Command care au drept target textBox-ul principal.
    /// </summary>
    public abstract class IMainTextBoxCommand : ISingletonCommand
    {
        abstract public void SetTarget(RichTextBoxV2 mainTextBox);
    }

    /// <summary>
    /// Interfața pentru toate obiectele de tip Singleton-Command care au drept target fereastra principală.
    /// </summary>
    public abstract class IMainWindowCommand : ISingletonCommand
    {
        abstract public void SetTarget(Form mainWindow);
    }

    /// <summary>
    /// Interfata pentru toate obiectele de tip Singleton-Command care au drept target tab-ul de fisiere
    /// </summary>
    public abstract class ITabControlCommand: ISingletonCommand
    {
        abstract public void SetTarget(TabControl tabControl);
    }

    /// <summary>
    /// Interfața pentru un subiect de observat.Implementează design patternul Observer alături de Observer.
    /// </summary>
    public interface ISubject
    {
        void Attach(IObserver observer);
        void NotifyObservers();
    }

    /// <summary>
    /// Interfața pentru un observator.Implementează design patternul Observer alături de Subject.
    /// </summary>
    public interface IObserver
    {
        void UpdateObserver();
    }
}