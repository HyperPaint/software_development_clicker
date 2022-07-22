namespace MyGame
{
    public delegate void Event<SENDER>(SENDER sender);
    public delegate void EventWith1Object<SENDER, OBJ1>(SENDER sender, OBJ1 obj1);
    public delegate void EventWith2Object<SENDER, OBJ1, OBJ2>(SENDER sender, OBJ1 obj1, OBJ2 obj2);
    public delegate void EventWith3Object<SENDER, OBJ1, OBJ2, OBJ3>(SENDER sender, OBJ1 obj1, OBJ2 obj2, OBJ3 obj3);
}
