using LibreHardwareMonitor.Hardware;

while (true)
{
    UpdateVisitor updateVisitor = new();
    Computer computer = new();
    computer.Open();
    computer.IsCpuEnabled = true;
    computer.Accept(updateVisitor);
    foreach (var hardware in computer.Hardware)
    {
        if (hardware.HardwareType is HardwareType.Cpu)
        {
            foreach (var sensor in hardware.Sensors)
            {
                if (sensor.SensorType == SensorType.Temperature)
                    Console.Write("{0} : {1:F}\t", sensor.Name, sensor.Value);
            }

            Thread.Sleep(500);
            Console.WriteLine();
        }
    }

    computer.Close();
}

internal class UpdateVisitor : IVisitor
{
    public void VisitComputer(IComputer computer) => computer.Traverse(this);

    public void VisitHardware(IHardware hardware)
    {
        hardware.Update();
        foreach (var subHardware in hardware.SubHardware) subHardware.Accept(this);
    }

    public void VisitSensor(ISensor sensor)
    {
    }

    public void VisitParameter(IParameter parameter)
    {
    }
}