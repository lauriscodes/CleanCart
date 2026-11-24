using Infrastructure.Composition;
using ServiceComposition.NET;

namespace Presentation.Console;

internal sealed class PresentationServiceComposition : ServiceCompositionRoot<AppServiceRegistrationPipeline>
{
}