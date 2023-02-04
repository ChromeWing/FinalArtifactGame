using Godot;
namespace ChromeWing.Temple{
	public class Singleton<T> where T:new(){
		private static T inst;
		protected static Node node{get{
			return Init.Instance;
		}}
		public static T Instance {get{
			if(inst==null){
				inst = new T();
			}
			return inst;
		}}

		public static void EnsureExists(){
			if(inst==null){
				inst = new T();
			}
		}
	}

}

