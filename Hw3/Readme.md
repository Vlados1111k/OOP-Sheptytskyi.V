# Самостійна робота №3

**ISP**: клієнти не повинні залежати від методів, які не використовують.

**DIP**: залежності мають будуватись на абстракціях.

---

### 1. Порушення ISP

* **Суть:** Інтерфейс містить зайві методи.

```csharp id="isp1"
interface IWorker
{
    void Work();
    void Eat();
}

class Robot : IWorker
{
    public void Work() { }
    public void Eat() => throw new NotImplementedException();
}
```

* **Рішення:** Розділити інтерфейс.

```csharp id="isp2"
interface IWorkable { void Work(); }
interface IEatable { void Eat(); }

class Robot : IWorkable
{
    public void Work() { }
}
```

---

### 2. DIP (Dependency Injection)

* **Суть:** Залежність від конкретного класу.

```csharp id="dip1"
class Switch
{
    private LightBulb bulb = new LightBulb();
}
```

* **Рішення:** Через інтерфейс і DI.

```csharp id="dip2"
interface ISwitchable { void TurnOn(); }

class Switch
{
    private readonly ISwitchable device;
    public Switch(ISwitchable device) => this.device = device;
}
```

---

### Висновок

**ISP** - вузькі інтерфейси

**DIP** - слабка зв’язаність, легке тестування
