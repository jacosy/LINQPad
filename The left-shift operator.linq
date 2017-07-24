<Query Kind="Program" />

// for more information, go to this link --> https://msdn.microsoft.com/en-us/library/a1sway8w.aspx
void Main()
{
	OverFlowSample();
	//MySample();
	//MSDNSampleCode();
}

// Define other methods and classes here
private void OverFlowSample()
{
	// low-order five bits
	int num = 1;
	Console.WriteLine(num << 1);	// 2, 1 = 00000
	Console.WriteLine("0x{0:x}", num << 1);     // 10 = 0x2 = 2
	Console.WriteLine(num << 2);    // 4, 2 = 00001
	Console.WriteLine("0x{0:x}", num << 2);     // 100 = 0x4 = 4

	Console.WriteLine(num << 33);    // 2, 33 = 100001, so the low-order five bits = 00001
	Console.WriteLine("0x{0:x}", num << 33);     // 10 = 0x2 = 2
}

private void MySample()
{
	int num = 11;   // 1011
	Console.WriteLine(num << 1);
	Console.WriteLine("0x{0:x}", num << 1);     // 10110 = 0x16 = 22
	Console.WriteLine(num << 2);
	Console.WriteLine("0x{0:x}", num << 2);     // 101100 = 0x2c = 44
}

private void MSDNSampleCode()
{
	int i = 1;
	long lg = 1;
	// Shift i one bit to the left. The result is 2.
	Console.WriteLine("0x{0:x}", i << 1);
	// In binary, 33 is 100001. Because the value of the five low-order
	// bits is 1, the result of the shift is again 2. 
	Console.WriteLine("0x{0:x}", i << 33);
	// Because the type of lg is long, the shift is the value of the six
	// low-order bits. In this example, the shift is 33, and the value of
	// lg is shifted 33 bits to the left.
	//     In binary:     10 0000 0000 0000 0000 0000 0000 0000 0000 
	//     In hexadecimal: 2    0    0    0    0    0    0    0    0
	Console.WriteLine("0x{0:x}", lg << 33);
}