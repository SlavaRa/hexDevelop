package test;

/**
 * ...
 * @author Christoph Otter
 */
class TestClass
{
	public static var xml : String;
	
	public static function main () : Void
	{
		var v : TestEnum;
	}

	public function new ()
	{
	}
	
	public static function test () : Void
	{
		
	}
	
}

typedef TestTypedef = {
	function test () : Void;
	function testTest (a : String) : Bool;
}

abstract TestAbstract (Int)
{
	public function new ()
	{
		this = 0;
	}
	
	public function test () : Void
	{
		trace ("test");
	}
}

enum TestEnum
{
	Test1;
}
